using Framework.Server.WebAppFramework;
using Framework.Server.WebAppFramework.Abstractions;
using System.Net;

namespace Framework.Server.WebAppFramework.MiddleWares;

public class EndpointMiddleWare : IMiddleWare
{
    public RequestDelegate? Next { get; set; }

    public async Task Invoke(HttpListenerContext context)
    {
        await Task.Run(() =>
        {
            Monitor.Enter(context);
            Console.WriteLine($"Прошли все промежуточные программы для данного запроса:");
            Console.WriteLine($"URL: {context.Request.Url}");
            Console.WriteLine($"Method: {context.Request.HttpMethod}");
            Console.WriteLine($"Code: {context.Response.StatusCode}");
            Monitor.Exit(context);
        });
        
    }
}