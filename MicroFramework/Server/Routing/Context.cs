using System.Net;

namespace Framework.Server.Routing;

public class Context
{
    public HttpListenerContext HttpContext { get; set; }
    public ServerSettings Settings { get; set; }

    public Context(HttpListenerContext httpContext, ServerSettings settings)
    {
        HttpContext = httpContext;
        Settings = settings;
    }
}
