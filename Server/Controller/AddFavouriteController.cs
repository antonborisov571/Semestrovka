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

[Controller("addfavourite")]
class AddFavouriteController
{

    [HttpPOST("/")]
    public async Task<IControllerResult> AddFavourite(string content,
        [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
    {
        var messageId = int.Parse(content);
        if (sessionId != default)
        {
            var session = await SessionManager.Instance.GetSession(sessionId);
            if (session is not null)
            {
                var favourites = await MyORM.Instance.Select<Favourite>();
                var favourite = favourites.FirstOrDefault(x => x.MessageId == messageId && x.UserId == session.AccountId);
                if (favourite != default)
                {
                    favourite.HasFavourite = false == favourite.HasFavourite ? true : false;
                    await MyORM.Instance.Update(favourite);
                }
                else
                {
                    await MyORM.Instance.Insert(new Favourite
                    {
                        UserId = session.AccountId,
                        MessageId = messageId,
                        DateDispatch = DateTime.Now,
                        HasFavourite = true
                    });
                }
                return new Ok();
            }
        }
        return new Redirect("/register");
    }
}
