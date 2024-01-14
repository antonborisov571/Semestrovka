using System.Net;

namespace Framework.Server.Extensions;

public static class ResponseExtesions
{
    public static void WriteBody(this HttpListenerResponse response, byte[] buffer, string contentType)
    {
        response.ContentLength64 = buffer.Length;
        response.ContentType = contentType;

        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }
}
