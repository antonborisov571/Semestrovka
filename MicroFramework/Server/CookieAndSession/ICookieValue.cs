using Framework.Server.Responses;
using System.Net;

namespace Framework.Server.CookieAndSession;

public interface ICookieValue
{
    IControllerResult IfNotExists { get; }
    Cookie AsCookie();
    Cookie AsCookie(TimeSpan expires);
}
