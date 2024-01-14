using Framework.Server.Routing;

namespace Framework.Server.Responses;

public class Ok : IControllerResult
{
    public Ok() { }

    public void ExecuteResult(Context context)
    {
        context.HttpContext.Response.StatusCode = 200;
    }
}
