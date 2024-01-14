using Framework.Server.Extensions;
using Framework.Server.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Server;

class DefaultResponseSender
{
    public bool SendStaticFile(Context context)
    {
        var ctx = context.HttpContext;
        var req = ctx.Request;
        var res = ctx.Response;
        var filePath = GetFilePath(req.RawUrl?.Substring(1), context.Settings);
        if (!File.Exists(filePath))
        {
            return false;
        }

        var buffer = File.ReadAllBytes(filePath);
        res.WriteBody(buffer, ContentTypeIdentifier.GetContentType(filePath));
        res.StatusCode = 200;

        return true;
    }

    public string GetFilePath(string? rawUrl, ServerSettings settings)
    {
        var filePath = Path.Combine(settings.TemplateFolder, rawUrl);
        return filePath;
    }
}
