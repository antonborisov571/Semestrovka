using System.Net;

namespace Framework.Server.WebAppFramework;

public delegate Task RequestDelegate(HttpListenerContext context);
