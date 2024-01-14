using Framework.AccountData;
using Framework.Server.CookieAndSession;
using Framework.Server.Helpers;
using Framework.Server.Responses;
using Framework.Server.Routing.Attributes;
using ORM.ORMPostgres;
using Server.PartialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controller
{
    [Controller()]
    public class Login
    {
        MyORM myOrm = MyORM.Instance;

        [HttpGET("/")]
        public IControllerResult LoginGet(
            [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
        {
            if (sessionId != default)
                return new Redirect("/profile");
            return new View("login", new LoginRegisterResultDto());
        }

        [HttpPOST("/")]
        public async Task<IControllerResult> LoginPostAsync(string login, string password,
            [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId,
            bool remember_me)
        {
            if (sessionId != default)
                return new Redirect("/profile");
            if (!InputFieldsValidator.ValidateLoginString(login))
                return new View("login",
                    new LoginRegisterResultDto { ErrorFieldName = "login", ErrorMsg = "Недопустимый формат у логина" });
            if (!InputFieldsValidator.ValidatePasswordString(password, out var errMsg))
                return new View("login",
                    new LoginRegisterResultDto { ErrorFieldName = "password", ErrorMsg = errMsg });

            var account = await AccountsData.GetAccountByLoginAndPassword((Model.Account)Activator.CreateInstance(typeof(Model.Account)),login, password);
            if (account is null)
                return new View("login",
                    new LoginRegisterResultDto { ErrorFieldName = "total", ErrorMsg = "Введен неправильный логин или пароль" });


            TimeSpan expiresIn;
            Session session;
            if (remember_me)
            {
                session = await SessionManager.Instance.CreateUnlimitedSession(account.Id, login);
                expiresIn = TimeSpan.FromDays(2);
            }
            else
            {
                expiresIn = TimeSpan.FromMinutes(2);
                session = await SessionManager.Instance.CreateSession(account.Id, login, expiresIn);
            }

            var cookies = new List<(ICookieValue, TimeSpan)>();
            cookies.Add((new SessionId { Id = session.Id }, expiresIn));
            var cookieResult = new CookieResult(cookies);
            return new ComposeResult(cookieResult,
                new Redirect("/"));
        }
    }
}
