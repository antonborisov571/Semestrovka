using Framework.Server.WebAppFramework.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Server.WebAppFramework.MiddleWares;

public class StaticFiles : IMiddleWare
{
    public RequestDelegate? Next { get; set; }

    public async Task Invoke(HttpListenerContext context)
    {
        var type = ContentTypeIdentifier.GetContentType(context.Request.Url.LocalPath);
        context.Response.ContentType = type;
        var file = await File.ReadAllBytesAsync($"{context.Request.Url.LocalPath}");
        context.Response.ContentLength64 = file.Length;
        context.Response.StatusCode = 200;
        await context.Response.OutputStream.WriteAsync(file);
        context.Response.OutputStream.Close();
        
        
    }
}
