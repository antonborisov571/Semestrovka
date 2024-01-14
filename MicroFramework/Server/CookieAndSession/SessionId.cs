using Framework.Server.Responses;
using System.Net;
using System.Text.Json.Serialization;

namespace Framework.Server.CookieAndSession;

public class SessionId : ICookieValue
{
    public Guid Id { get; set; }

    [JsonIgnore]
    public IControllerResult IfNotExists { get; } = new NotAuthorized();

    public Cookie AsCookie()
    {
        var value = CookieValueSerializer.Serialize(this);
        return new Cookie() { Name = "SessionId", Value = value };
    }

    public Cookie AsCookie(TimeSpan expires)
    {
        var value = CookieValueSerializer.Serialize(this);
        return new Cookie() { Name = "SessionId", Value = value, Expires = DateTime.Now + expires };
    }
}
