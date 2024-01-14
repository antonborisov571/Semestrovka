namespace Framework.Server.Routing.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ControllerAttribute : Attribute
{
    public string Uri { get; set; }

    public ControllerAttribute()
    {
    }

    public ControllerAttribute(string uri)
    {
        Uri = uri;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public abstract class HttpMethodAttribute : Attribute 
{
    public string Uri { get; set; }

    public HttpMethodAttribute()
    {
    }

    public HttpMethodAttribute(string uri)
    {
        Uri = uri;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class HttpGET : HttpMethodAttribute
{
    public HttpGET() : base() 
    { }

    public HttpGET(string uri) : base(uri) 
    { }
}

[AttributeUsage(AttributeTargets.Method)]
public class HttpPOST : HttpMethodAttribute
{
    public HttpPOST() : base()
    { }

    public HttpPOST(string uri) : base(uri)
    { }
}

[AttributeUsage(AttributeTargets.Method)]
public class HttpPUT : HttpMethodAttribute
{
    public HttpPUT() : base()
    { }

    public HttpPUT(string uri) : base(uri)
    { }
}

[AttributeUsage(AttributeTargets.Method)]
public class HttpDELETE : HttpMethodAttribute
{
    public HttpDELETE() : base()
    { }

    public HttpDELETE(string uri) : base(uri)
    { }
}
