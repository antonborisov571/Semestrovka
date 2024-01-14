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

[Controller()]
public class Favourites
{
    [HttpGET("/")]
    public async Task<IControllerResult> GetFavourites([FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
    {
        if (sessionId != default)
        {
            var session = await SessionManager.Instance.GetSession(sessionId);
            if (session is not null)
            {
                var accounts = await MyORM.Instance.Select<Account>();
                var favourites = (await MyORM.Instance.Select<Favourite>()).Where(x => x.UserId == session.AccountId && x.HasFavourite);
                var messages = (await MyORM.Instance.Select<Message>()).Where(x => favourites.Select(y => y.MessageId).Contains(x.Id))
                    .Select(x => new
                    {
                        MessageInfo = x,
                        User = accounts.FirstOrDefault(y => x.UserId == y.Id),
                        HasFavourite = favourites.Any(y => y.MessageId == x.Id && y.HasFavourite)
                    });
                return new View("favourites", new
                {
                    Messages = messages
                });
            }
        }
        return new Redirect("/login");
    }
}
