using Framework.AccountData;
using Framework.Server.CookieAndSession;
using Framework.Server.Responses;
using Framework.Server.Routing.Attributes;
using ORM.ORMPostgres;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controller;

[Controller("setlike")]
class SetLikeController
{

    [HttpPOST("/")]
    public async Task<IControllerResult> SetLike(string content,
        [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
    {
        var messageId = int.Parse(content);
        if (sessionId != default)
        {
            var session = await SessionManager.Instance.GetSession(sessionId);
            if (session is not null)
            {
                var likes = await MyORM.Instance.Select<Like>();
                var like = likes.FirstOrDefault(x => x.MessageId == messageId && x.UserId == session.AccountId);
                
                if (like != default)
                {
                    like.HasLike = false == like.HasLike ? true : false;
                    await MyORM.Instance.Update(like);
                }
                else
                {
                    var userId = (await MyORM.Instance.Select<Message>()).FirstOrDefault(x => x.Id == messageId)!.UserId;
                    var accountSender = (await MyORM.Instance.Select<Model.Account>()).FirstOrDefault(x => x.Id == userId);
                    accountSender!.Reputation++;
                    await MyORM.Instance.Update(accountSender);
                    await MyORM.Instance.Insert(new Like
                    {
                        UserId = session.AccountId,
                        MessageId = messageId,
                        DateDispatch = DateTime.Now,
                        HasLike = true
                    });
                }
                return new Ok();
            }
        }
        return new Redirect("/register");
    }
}
