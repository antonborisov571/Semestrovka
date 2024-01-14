using Framework.Server.CookieAndSession;
using Framework.Server.Responses;
using Framework.Server.Routing.Attributes;
using ORM.ORMPostgres;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controller;

[Controller("deletemessage")]
class DeleteMessageController
{
    [HttpPOST("/")]
    public async Task<IControllerResult> DeleteMessage(string content,
        [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
    {
        var messageId = int.Parse(content);
        if (sessionId != default)
        {
            var session = await SessionManager.Instance.GetSession(sessionId);
            if (session is not null)
            {
                var message = (await MyORM.Instance.Select<Message>()).FirstOrDefault(x => x.Id == messageId);
                var post = (await MyORM.Instance.Select<Post>()).FirstOrDefault(x => x.Id == message.PostId);
                await MyORM.Instance.Delete(message);
                return new Ok();
            }
        }
        return new Redirect("/register");
    }
}
