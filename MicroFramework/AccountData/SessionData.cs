using ORM.ORMPostgres;
using Npgsql;
using System.Data;
using System.Reflection;
using Framework.Server.CookieAndSession;
using Framework.Server.Extensions;

namespace Framework.AccountData;

public class SessionData
{
    string connectionString;
    static Dictionary<Type, string> tableNames;
    static Dictionary<PropertyInfo, string> columnNames;
    static Dictionary<(Type, string), PropertyInfo> propertyName;

    static SessionData()
    {
        tableNames = new Dictionary<Type, string>();
        columnNames = new Dictionary<PropertyInfo, string>();
        propertyName = new Dictionary<(Type, string), PropertyInfo>();

        var tableName = typeof(Session)
            .GetCustomAttributes<Table>(false)
            .FirstOrDefault(new Table(nameof(Session) + 's'));
        tableNames[typeof(Session)] = tableName.Name;

        var properties = typeof(Session).GetProperties();
        foreach (var property in properties)
        {
            var columnName = property
                .GetCustomAttributes<Column>(false)
                .FirstOrDefault(new Column(property.Name));
            columnNames[property] = columnName.Name;

            var key = (property.DeclaringType, property.Name);
            if (propertyName.ContainsKey(key))
            {
                throw new InvalidConstraintException($"Внутри модели не могут быть свойства с одним и тем же именем: {property.Name}");
            }
            propertyName[key] = property;
        }
    }

    private SessionData(string connectionString)
    {
        this.connectionString = connectionString;
    }

    private static Lazy<SessionData> instance;

    public static void Init(string connectionString) =>
        instance = new Lazy<SessionData>(() => new SessionData(connectionString));

    public static SessionData Instance => instance.Value;

    public async Task<int> ExecuteNonQuery(string query)
    {
        int affectedRows = default;
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            var command = new NpgsqlCommand(query, connection);
            try
            {
                affectedRows = await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        return affectedRows;
    }

    public async Task<int> Insert(Session obj)
    {
        var properties = typeof(Session).GetProperties();
        var tableColumnNames = properties.Select(x => columnNames[x]);

        var query = $"insert into {tableNames[typeof(Session)]} ({string.Join(", ", tableColumnNames)})\n" +
            $"values ({string.Join(", ", properties.Select(x => x.GetValue(obj).GetSqlStringValue()))})";

        return await ExecuteNonQuery(query);
    }

    public async Task<int> Update(Session obj)
    {
        var properties = typeof(Session).GetProperties().Where(x => x.Name != "AccountId");
        var propertyNamesInTable = properties.Select(x => columnNames[x]);
        var query = $"update {tableNames[typeof(Session)]}\n" +
        $"set {string.Join(", ", properties.Zip(propertyNamesInTable).Select(x => $"{x.Second} = {x.First.GetValue(obj).GetSqlStringValue()}"))}\n" +
        $"where AccountId = '{obj.AccountId}'";

        return await ExecuteNonQuery(query);
    }

    public async Task<int> Delete(Session obj)
    {
        var properties = typeof(Session).GetProperties().Where(x => x.Name != "Expires");
        var propertyNamesInTable = properties.Select(x => columnNames[x]);
        var query = $"delete from {tableNames[typeof(Session)]}\n " +
        $"where {string.Join(" and ", properties.Zip(propertyNamesInTable).Select(x => $"{x.Second} = {x.First.GetValue(obj).GetSqlStringValue()}"))}";

        return await ExecuteNonQuery(query);
    }

    public async Task<IEnumerable<Session>> Select()
    {
        var result = new List<Session>();

        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            var command = new NpgsqlCommand($"select * from {tableNames[typeof(Session)]}", connection);
            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                var properties = typeof(Session).GetProperties();
                var propertyNamesInTable = properties.Select(x => columnNames[x]);

                while (reader.Read())
                {
                    var obj = Activator.CreateInstance(typeof(Session));
                    foreach (var property in properties.Zip(propertyNamesInTable))
                    {
                        var value = reader.GetValue(reader.GetOrdinal(property.Second));
                        if (value is DBNull)
                        {
                            value = null;
                        }
                        property.First.SetValue(obj, value);
                    }
                    result.Add((Session)obj);
                }
            }
        }
        return result;
    }

    public async Task<Session?> GetSessionById(Guid id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            var command = new NpgsqlCommand($"select * from {tableNames[typeof(Session)]} where Id = '{id}'", connection);
            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                var properties = typeof(Session).GetProperties();
                var propertyNamesInTable = properties.Select(x => columnNames[x]);

                while (reader.Read())
                {
                    var obj = Activator.CreateInstance(typeof(Session));
                    foreach (var property in properties.Zip(propertyNamesInTable))
                    {
                        var value = reader.GetValue(reader.GetOrdinal(property.Second));
                        if (value is DBNull)
                        {
                            value = null;
                        }
                        property.First.SetValue(obj, value);
                    }
                    return (Session)obj;
                }
            }
        }
        return null;
    }

    public async Task<Session?> GetSessionByAccountId(int accountId)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            var command = new NpgsqlCommand($"select * from {tableNames[typeof(Session)]} where AccountId = {accountId}", connection);
            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                var properties = typeof(Session).GetProperties();
                var propertyNamesInTable = properties.Select(x => columnNames[x]);

                while (reader.Read())
                {
                    var obj = Activator.CreateInstance(typeof(Session));
                    foreach (var property in properties.Zip(propertyNamesInTable))
                    {
                        var value = reader.GetValue(reader.GetOrdinal(property.Second));
                        if (value is DBNull)
                        {
                            value = null;
                        }
                        property.First.SetValue(obj, value);
                    }
                    return (Session)obj;
                }
            }
        }
        return null;
    }
}
