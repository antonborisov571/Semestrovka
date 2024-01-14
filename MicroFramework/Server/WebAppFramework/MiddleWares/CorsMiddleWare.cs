using Framework.Server.WebAppFramework.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Server.WebAppFramework.MiddleWares;

public class CorsMiddleWare : IMiddleWare
{
    public RequestDelegate? Next { get; set; }

    public Task Invoke(HttpListenerContext context)
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        return Task.CompletedTask;
    }
}
