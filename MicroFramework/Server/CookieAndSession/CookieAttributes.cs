namespace Framework.Server.CookieAndSession;

[AttributeUsage(AttributeTargets.Method)]
public class CheckCookie : Attribute
{
    public Type Type { get; }
    public string Name { get; }
    public object? Value { get; }
    public CheckCookie(Type type)
    {
        if (!typeof(ICookieValue).IsAssignableFrom(type))
        {
            throw new ArgumentException($"Нельзя {type.Name} привести к классу который реализует ICookieValue");
        }
        Type = type;
    }

    public CheckCookie(Type type, string name, object? value) : this(type)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"Имя не может быть пустым");
        }
        Name = name;
        Value = value;
    }
}

[AttributeUsage(AttributeTargets.Parameter)]
public class FromCookie : Attribute
{
    public Type Type { get; }
    public string PropertyName { get; }

    public FromCookie(Type type)
    {
        if (!typeof(ICookieValue).IsAssignableFrom(type))
        {
            throw new ArgumentException($"Нельзя {type.Name} привести к классу который реализует ICookieValue");
        }
        Type = type;
    }

    public FromCookie(Type type, string propertyName)
        : this(type)
    {
        if (string.IsNullOrEmpty(propertyName))
            throw new ArgumentException("Имя не может быть пустым");
        PropertyName = propertyName;
    }
}
