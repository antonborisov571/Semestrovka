using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Npgsql;
using ORM.Extensions;
using ORM.ORMPostgres;
using ORM.ORMPostgres.BuilderType;
using ORM.ORMPostgres.Helpers;
using ORM.ORMPostgres.QueryProvider;

namespace ORM.ORMPostgres;

public class MyORM
{
    static DBConnection Connection { get; set; }
    static Dictionary<Type, string> tableNames;
    static Dictionary<PropertyInfo, string> columnByNames;
    static Dictionary<(Type, string), PropertyInfo> propertyByColumnName;

    static MyORM()
    {
        tableNames = new Dictionary<Type, string>();
        columnByNames = new Dictionary<PropertyInfo, string>();
        propertyByColumnName = new Dictionary<(Type, string), PropertyInfo>();
        GetTableModels();
    }

    private static void GetTableModels()
    {
        var types = Assembly
            .GetEntryAssembly()!
            .GetTypes()
            .Where(x => x.GetInterfaces().Any(y => y.Equals(typeof(IEntity))));

        foreach (var type in types)
        {
            var tableName = type
                .GetCustomAttributes<Table>(false)
                .FirstOrDefault(new Table(type.Name + 's'));
            tableNames[type] = tableName.Name;
        }

        var properties = types.SelectMany(x => x.GetProperties());
        foreach (var property in properties)
        {
            var columnName = property
                .GetCustomAttributes<Column>(false)
                .FirstOrDefault(new Column(property.Name));
            columnByNames[property] = columnName.Name;

            var key = (property.DeclaringType, property.Name);
            if (propertyByColumnName.ContainsKey(key))
            {
                throw new InvalidConstraintException($"Внутри модели не могут быть свойства с одним и тем же именем: {property.Name}");
            }
            propertyByColumnName[key] = property;
        }
    }

    private MyORM(string connectionString)
    {
        Connection = new DBConnection(connectionString);
    }

    private static Lazy<MyORM> instance;

    public static void Init(string connectionString) =>
        instance = new Lazy<MyORM>(() => new MyORM(connectionString));

    public static MyORM Instance => instance.Value;

    public async Task<int> Insert<TEntity>(TEntity obj) where TEntity : IEntity
    {
        var properties = typeof(TEntity).GetProperties().Where(x => x.Name != "Id");
        var columnNames = properties.Select(x => columnByNames[x]);

        var query = $"insert into {tableNames[typeof(TEntity)]} ({string.Join(", ", columnNames)})\n" +
            $"values ({string.Join(", ", properties.Select(x => x.Name.GetSqlStringByParameter()))})";
        var parameters = properties.GetNpgsqlParameters(obj);

        return await ExecuteNonQuery(query, parameters);
    }

    public async Task<int> Update<TEntity>(TEntity obj) where TEntity : IEntity
    {
        var properties = typeof(TEntity).GetProperties().Where(x => x.Name != "Id");
        var columnNames = properties.Select(x => columnByNames[x]);

        var query = $"update {tableNames[typeof(TEntity)]}\n" +
            $"set {string.Join(", ", properties
            .Zip(columnNames)
            .Select(x => x.Second + " = " + x.First.Name.GetSqlStringByParameter()))}" +
            $" where Id = {obj.Id}";
        var parameters = properties.GetNpgsqlParameters(obj);

        return await ExecuteNonQuery(query, parameters);
    }

    public async Task<int> Delete<TEntity>(TEntity obj) where TEntity : IEntity
    {
        var properties = typeof(TEntity).GetProperties().Where(x => x.Name != "Id");
        var columnNames = properties.Select(x => columnByNames[x]);

        var query = $"delete from {tableNames[typeof(TEntity)]}\n" +
            $"where {string.Join(" and ", properties
        .Zip(columnNames)
            .Select(x => x.Second + " = " + x.First.Name.GetSqlStringByParameter()))}";
        var parameters = properties.GetNpgsqlParameters(obj);


        return await ExecuteNonQuery(query, parameters);
    }

    public async Task<IEnumerable<TEntity>> Select<TEntity>() where TEntity : IEntity
    {
        var result = new List<TEntity>();   
        
        using (var connection = Connection.GetConnection())
        {
            await connection.OpenAsync();
            var command = new NpgsqlCommand($"select * from {tableNames[typeof(TEntity)]}", connection);
            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                var properties = typeof(TEntity).GetProperties();
                var propertyNamesInTable = properties.Select(x => columnByNames[x]);

                while (reader.Read())
                {
                    var obj = Activator.CreateInstance(typeof(TEntity));
                    foreach(var property in properties.Zip(propertyNamesInTable))
                    {
                        var value = reader.GetValue(reader.GetOrdinal(property.Second));
                        if (value is DBNull)
                        {
                            value = null;
                        }
                        property.First.SetValue(obj, value);
                    }
                    result.Add((TEntity)obj);
                }
            }
        }
        return result;
    }

    public async Task<IEnumerable<dynamic>> Select<TEntity>(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> func) where TEntity : IEntity
    {
        var result = new List<dynamic>();
        
        using (var connection = Connection.GetConnection())
        {
            await connection.OpenAsync();
            var provider = new MyORMQueryProvider();
            var sql = provider.Execute(func, tableNames[typeof(TEntity)]);
            var command = new NpgsqlCommand(sql.ToString(), connection);
            var reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                var properties = typeof(TEntity).GetProperties();
                var propertyNamesInTable = properties.Select(x => columnByNames[x]);
                while (reader.Read())
                {
                    var obj = Activator.CreateInstance(typeof(TEntity));
                    foreach (var property in properties.Zip(propertyNamesInTable))
                    {
                        if (reader.TryGetOrdinal(property.Second, out int ordinal))
                        {
                            var value = reader.GetValue(ordinal);
                            if (value is DBNull)
                            {
                                value = null;
                            }
                            property.First.SetValue(obj, value);
                        }
                    }
                    result.Add(obj);
                }
            }
        }
        return result;
    }

    public async Task<IEnumerable<TEntity>> Select<TEntity>(TEntity entity) where TEntity : IEntity
    {
        var result = new List<TEntity>();

        using (var connection = Connection.GetConnection())
        {
            var properties = typeof(TEntity).GetProperties();
            var propertyNamesInTable = properties.Select(x => columnByNames[x]);
            var sb = new StringBuilder($"select * from {tableNames[typeof(TEntity)]} where ");
            var isFirstProperty = true;
            foreach (var property in properties.Zip(propertyNamesInTable))
            {
                
                if (property.First is not null && property.First.Name != "Id")
                {
                    if (!isFirstProperty)
                    {
                        sb.Append(" and ");
                    }

                    sb.AppendLine($"{property.Second} = {property.First.GetValue(entity).GetSqlStringValue()}");
                    isFirstProperty = false;
                }
                    

            }
            sb.Append(";");
            await connection.OpenAsync();
            var command = new NpgsqlCommand(sb.ToString(), connection);
            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                

                while (reader.Read())
                {
                    var obj = Activator.CreateInstance(typeof(TEntity));
                    foreach (var property in properties.Zip(propertyNamesInTable))
                    {
                        var value = reader.GetValue(reader.GetOrdinal(property.Second));
                        if (value is DBNull)
                        {
                            value = null;
                        }
                        property.First.SetValue(obj, value);
                    }
                    result.Add((TEntity)obj);
                }
            }
        }
        return result;
    }

    public async Task<IEnumerable<object>> SelectCrossJoin<TEntity, VEntity>() 
        where TEntity : IEntity
        where VEntity : IEntity
    {
        var result = new List<object>();

        using (var connection = Connection.GetConnection())
        {
            await connection.OpenAsync();
            var command = new NpgsqlCommand($"select * from {tableNames[typeof(TEntity)] + "s"} cross join {tableNames[typeof(VEntity)] + "s"}", connection);
            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                var builderType = new TypesBuilder();
                var tb = builderType.GetTypeBuilder(tableNames[typeof(TEntity)] + "s" + tableNames[typeof(VEntity)] + "s");
                foreach (var property in reader.GetColumnSchema())
                {
                    builderType.CreateProperty(tb, property.ColumnName, property.DataType);
                }
                var table = tb.CreateType();
                var properties = table.GetProperties();
                var propertyNamesInTable = properties.Select(x => x.Name);

                while (reader.Read())
                {
                    var obj = Activator.CreateInstance(table);
                    foreach (var property in properties.Zip(propertyNamesInTable))
                    {
                        var value = reader.GetValue(reader.GetOrdinal(property.Second));
                        if (value is DBNull)
                        {
                            value = null;
                        }
                        property.First.SetValue(obj, value);
                    }
                    result.Add(obj);
                }
            }
        }
        return result;
    }

    public async Task<int> CreateTable<TEntity>(TEntity obj) where TEntity : IEntity
    {
        var properties = obj.GetType().GetProperties().Where(x => x.Name.ToLower() != "id");
        var columnNames = properties.Select(x => columnByNames[x]);
        var query = new StringBuilder();
        query = query.Append($"create table if not exists {tableNames[obj.GetType()] + "s"}(\n")
            .Append("id serial primary key,\n")
            .Append(string.Join(", ", properties.Zip(columnNames)
            .Select(x => x.Second.ToLower() + " " + x.First.PropertyType.MapSQLType())))
            .Append(")");


        return await ExecuteNonQuery(query.ToString());
    }

    public async Task<int> CreateTable<TEntity>() where TEntity : IEntity
    {
        var properties = typeof(TEntity).GetProperties().Where(x => x.Name.ToLower() != "id");
        var columnNames = properties.Select(x => columnByNames[x]);
        var query = new StringBuilder();
        query = query.Append($"create table if not exists {tableNames[typeof(TEntity)] + "s"}(\n")
            .Append("id serial primary key,\n")
            .Append(string.Join(", ", properties.Zip(columnNames)
            .Select(x => x.Second.ToLower() + " " + x.First.PropertyType.MapSQLType())))
            .Append(")");


        return await ExecuteNonQuery(query.ToString());
    }

    public async Task CreateDatabase()
    {
        foreach (var (key, value) in tableNames)
        {
            await CreateTable((IEntity)Activator.CreateInstance(key));
        }
    }

    private static string ReplaceParameters(string query)
    {
        var result = Regex.Replace(query, @"\{(\d+)\}", x => $"@p{x.Groups[1].Value}");
        return result;
    }

    private async Task<int> ExecuteNonQuery(string query)
    {
        int affectedRows = default;
        using (var connection = Connection.GetConnection())
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction()) 
            {
                var command = new NpgsqlCommand(query, connection);
                command.Transaction = transaction;
                try
                {
                    affectedRows = await command.ExecuteNonQueryAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                }
                await connection.CloseAsync();
            }
            
        }
        return affectedRows;
    }

    private async Task<int> ExecuteNonQuery(string query, List<NpgsqlParameter> parameters)
    {
        int affectedRows = default;
        using (var connection = Connection.GetConnection())
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddRange(parameters.ToArray());
                command.Transaction = transaction;
                try
                {
                    affectedRows = await command.ExecuteNonQueryAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                }
                await connection.CloseAsync();
            }

        }
        return affectedRows;
    }
}
