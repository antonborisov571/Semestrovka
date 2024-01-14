using Framework.Server.Extensions;
using Framework.Server.Routing;
using System.Text;

namespace Framework.Server.Responses;

public class NotFound : IControllerResult
{
    public void ExecuteResult(Context context)
    {
        var res = context.HttpContext.Response;
        res.StatusCode = 404;
        res.WriteBody(Encoding.UTF8.GetBytes("Not Found: 404"), "text/html; charset=UTF-8");
    }
}
