using Framework.Server.WebAppFramework.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Server.WebAppFramework.MiddleWares;

public class Authentication : IMiddleWare
{
    public RequestDelegate? Next { get; set; }

    public Task Invoke(HttpListenerContext context)
    {
        var response = context.Response;
        response.StatusCode = (int)HttpStatusCode.Unauthorized;
        response.AddHeader("WWW-Authenticate", "Basic realm=\"My Server\"");
        byte[] unauthorizedBytes = Encoding.UTF8.GetBytes("401 Unauthorized");
        response.OutputStream.Write(unauthorizedBytes, 0, unauthorizedBytes.Length);
        return Task.CompletedTask;
    }
}
