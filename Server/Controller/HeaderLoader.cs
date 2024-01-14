using Framework.AccountData;
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

[Controller("header")]
public class HeaderLoader
{
    [HttpGET("/")]
    public async Task<IControllerResult> GetHeaderAsync(
        [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
    {
        var model = "";
        if (sessionId != default)
        {
            var session = await SessionManager.Instance.GetSession(sessionId);
            if (session is not null)
            {
                model = (await AccountsData.GetAccountById((Model.Account)Activator.CreateInstance(typeof(Model.Account))!, session.AccountId))?
                    .Login ?? "";
                var account = (await MyORM.Instance.Select<Model.Account>()).FirstOrDefault(x => x.Id == session.AccountId);
                account.LastOnline = DateTime.Now;
                await MyORM.Instance.Update(account);
            }
                
        }
        return new View("header", model);
    }
}
