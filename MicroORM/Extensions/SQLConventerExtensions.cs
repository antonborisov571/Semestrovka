using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Extensions;

public static class SQLConverterExtension
{
    public static string GetSqlStringValue(this object obj)
    {
        if (obj is null)
            return "null";
        if (obj is bool)
            return $"'{obj.ToString()!.ToLower()}'";
        if (obj is DateTime date)
            return $"'{date.ToString("yyyy-MM-dd HH:mm:ss.fff")}'";
        if (obj is Enum)
            return $"'{(int)obj}'";
        return $"'{obj}'";
    }

    public static string GetSqlStringByParameter(this object obj) => $"@{obj}";

    public static List<NpgsqlParameter> GetNpgsqlParameters(this IEnumerable<PropertyInfo> properties, object obj)
    {
        var result = new List<NpgsqlParameter>();
        foreach (var property in properties)
        {
            var parameter = new NpgsqlParameter();
            parameter.ParameterName = $"{property.Name}";
            parameter.Value = property.GetValue(obj) ?? DBNull.Value;
            if (property.PropertyType == typeof(string))
                parameter.NpgsqlDbType = NpgsqlDbType.Varchar;
            if (property.PropertyType == typeof(bool))
                parameter.NpgsqlDbType = NpgsqlDbType.Boolean;
            if (property.PropertyType == typeof(DateTime))
                parameter.NpgsqlDbType = NpgsqlDbType.Timestamp;
            if (property.PropertyType == typeof(Enum))
                parameter.NpgsqlDbType = NpgsqlDbType.Integer;
            if (property.PropertyType == typeof(int))
                parameter.NpgsqlDbType = NpgsqlDbType.Integer;
            if (property.PropertyType == typeof(double))
                parameter.NpgsqlDbType = NpgsqlDbType.Double;
            if (property.PropertyType == typeof(Guid))
                parameter.NpgsqlDbType = NpgsqlDbType.Uuid;
            result.Add(parameter);
        }
        return result;
    }
}
