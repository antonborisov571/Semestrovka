using Framework.AccountData;
using Framework.Server.CookieAndSession;
using Framework.Server.Responses;
using Framework.Server.Routing.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controller
{
    [Controller("/profile")]
    public class ProfileController
    {
        [HttpGET("/")]
        public async Task<IControllerResult> ProfileViewerAsync(
            [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
        {
            Account? acc = default;
            if (sessionId != default)
            {
                var session = await SessionManager.Instance.GetSession(sessionId);
                if (session is not null)
                    acc = await AccountsData.GetAccountById((Model.Account)Activator.CreateInstance(typeof(Model.Account))!, session.AccountId);
            }
            if (acc is null)
                return new Redirect("/login");
            return new View("profile", acc);
        }

        [HttpPOST("/")]
        public async Task<IControllerResult> ExitAsync(
            [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
        {
            Account? acc = default;
            if (sessionId != default)
            {
                var session = await SessionManager.Instance.GetSession(sessionId);
                if (session is not null)
                    await SessionData.Instance.Delete(session);
            }
            return new Redirect("/profile");
        }
    }
}
