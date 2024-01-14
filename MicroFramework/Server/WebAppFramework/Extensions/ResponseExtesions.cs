using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Server.WebAppFramework.Extensions;

public static class ResponseExtesions
{
    public static async Task WriteBody(this HttpListenerResponse response, string content, string contentType)
    {
        var contentBytes = Encoding.UTF8.GetBytes(content);
        response.ContentLength64 = contentBytes.Length;
        response.ContentType = contentType;
        await response.OutputStream.WriteAsync(contentBytes, 0, contentBytes.Length);
    }

    public static async Task WriteBody(this HttpListenerResponse response, string content)
    {
        var contentBytes = Encoding.UTF8.GetBytes(content);
        response.ContentLength64 = contentBytes.Length;
        await response.OutputStream.WriteAsync(contentBytes, 0, contentBytes.Length);
    }
}
