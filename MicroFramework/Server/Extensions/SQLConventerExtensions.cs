namespace Framework.Server.Extensions;

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
}
