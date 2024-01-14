using Framework.Server.CookieAndSession;
using Framework.Server.Routing;

namespace Framework.Server.Responses;

public class CookieResult : IControllerResult
{
    List<(ICookieValue, TimeSpan)> CookieInfo { get; set; }

    public CookieResult(List<(ICookieValue, TimeSpan)> cookieInfo)
    {
        CookieInfo = cookieInfo;
    }

    public void ExecuteResult(Context context)
    {
        var response = context.HttpContext.Response;
        foreach (var (cookie, expires) in CookieInfo) 
        {
            if (expires != default)
            {
                response.Cookies.Add(cookie.AsCookie(expires));
            }
            else
            {
                response.Cookies.Add(cookie.AsCookie());
            }
        }
    }
}
