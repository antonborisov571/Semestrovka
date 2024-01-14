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

[Controller("newmessages")]
class NewMessages
{
    [HttpGET("/")]
    public async Task<IControllerResult> GetNewMessages([FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
    {
        var orm = MyORM.Instance;
        var accounts = await orm.Select<Account>();
        var posts = await orm.Select<Post>();
        var topics = await orm.Select<Topic>();
        var messages = await orm.Select<Message>();
        var favourites = (IEnumerable<Favourite>?)new List<Favourite>();
        var likes = (IEnumerable<Like>?)new List<Like>();

        var sessionInfo = sessionId.ToString();
        int currentUserId = 0;
        if (sessionId != default)
        {
            var session = await SessionManager.Instance.GetSession(sessionId);
            if (session is not null)
            {
                currentUserId = session.AccountId;
                favourites = (await MyORM.Instance.Select<Favourite>()).Where(x => x.UserId == session.AccountId);
                likes = (await MyORM.Instance.Select<Like>()).Where(x => x.UserId == session.AccountId);
            }
        }
        else
        {
            sessionInfo = "";
        }

        var preparedMessages = messages
            .Select(x =>
            new
            {
                User = accounts.FirstOrDefault(y => y.Id == x.UserId),
                MessageInfo = x,
                HasFavourite = favourites.Any(y => y.MessageId == x.Id && y.HasFavourite),
                HasLike = likes.Any(y => y.MessageId == x.Id && y.HasLike),
                IsOnline = accounts.FirstOrDefault(y => y.Id == x.UserId).LastOnline.AddMinutes(6) > DateTime.Now,
                PostInfo = posts.FirstOrDefault(y => y.Id == x.PostId),
            });

        return new View("newmessages",
            new
            {
                CountMessages = preparedMessages.Count(),
                Messages = preparedMessages.Take(100),
                SessionInfo = sessionInfo,
            });
    }
}
