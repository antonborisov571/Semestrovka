using Framework.Server.Routing;

namespace Framework.Server.Responses;

public class NotAuthorized : IControllerResult
{
    public void ExecuteResult(Context context)
    {
        var response = context.HttpContext.Response;
        response.StatusCode = 401;
    }
}
