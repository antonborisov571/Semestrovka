namespace Framework.Server.Routing;

class ArgType
{
    public Type Type { get; set; }
    public Func<string, object> Parser { get; set; }

    public static List<ArgType> argTypes = new List<ArgType>
    {
        new ArgType(typeof(bool), s => bool.Parse(s)),
        new ArgType(typeof(int), s => int.Parse(s)),
        new ArgType(typeof(double), s => double.Parse(s)),
        new ArgType(typeof(Guid), s => Guid.Parse(s)),
        new ArgType(typeof(string), s => s),
    };

    public ArgType(Type type, Func<string, object> parser)
    {
        Type = type;
        Parser = parser;
    }

    public static bool TryParse<T>(string str, out T result)
    {
        try
        {
            result = (T)GetArgType(typeof(T))?.Parser(str)!;
            return true;
        }
        catch 
        {
            result = default!;
            return false; 
        }
    }

    public static bool TryParse(string str, out object val, out Type type) 
    {
        for (int i = 0; i < argTypes.Count; i++)
        {
            var argType = argTypes[i];
            try
            {
                val = argType.Parser(str);
                type = argType.Type;
                return true;
            }
            catch
            {
                continue;
            }
        }

        val = default!;
        type = default!;
        return false;
    }

    public static ArgType? GetArgType(Type type) 
        => argTypes.FirstOrDefault(x => x.Type == type);

}
