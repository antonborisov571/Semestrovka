using System.Net;

namespace Framework.Server.WebAppFramework.Abstractions;

public interface IMiddleWare
{
    RequestDelegate? Next { get; set; }
    Task Invoke(HttpListenerContext context);
}