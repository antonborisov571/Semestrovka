using Framework.Server.Routing;

namespace Framework.Server.Responses;

public class BadRequest : IControllerResult
{
    public void ExecuteResult(Context context)
    {
        var res = context.HttpContext.Response;
        res.StatusCode = 400;
    }
}
