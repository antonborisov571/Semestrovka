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

[Controller("addimageprofile")]
class AddImageProfileController
{
    [HttpPOST("/")]
    public async Task<IControllerResult> AddImage(string content,
        [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
    {
        var accounts = await MyORM.Instance.Select<Model.Account>();
        Model.Account? account;
        if (sessionId != default)
        {
            var session = await SessionManager.Instance.GetSession(sessionId);
            if (session is not null)
            {
                account = accounts.FirstOrDefault(x => x.Id == session.AccountId);
                account.UrlImage = content;
                await MyORM.Instance.Update(account);
            }
        }
        return new Ok();
    }
}
