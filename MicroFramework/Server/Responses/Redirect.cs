using Framework.Server.Routing;

namespace Framework.Server.Responses;

public class Redirect : IControllerResult
{
    string RedirectUri { get; set; }

    public Redirect(string redirectUri)
    {
        RedirectUri = redirectUri;
    }

    public void ExecuteResult(Context context)
    {
        context.HttpContext.Response.RedirectLocation = RedirectUri;
        context.HttpContext.Response.StatusCode = 302;
    }
}
