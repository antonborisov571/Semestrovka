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
    [Controller]
    public class Register
    {
        [HttpGET("/")]
        public IControllerResult RegisterGet(
            [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
        {
            if (sessionId != default)
                return new Redirect("/profile");
            return new View("register", new LoginRegisterResultDto());
        }

        [HttpPOST("/")]
        public async Task<IControllerResult> RegisterPost(string login, string password,
            string repeat_password, string email, string tractorName,
            [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
        {
            if (sessionId != default)
                return new Redirect("profile");
            if (!InputFieldsValidator.ValidateLoginString(login))
                return new View("register",
                    new LoginRegisterResultDto { ErrorFieldName = "login", ErrorMsg = "Недопустимый формат у Логина" });
            if (!InputFieldsValidator.ValidatePasswordString(password, out var errMsg))
                return new View("register",
                    new LoginRegisterResultDto { ErrorFieldName = "password", ErrorMsg = errMsg });
            if (!InputFieldsValidator.ValidateEmailString(email))
                return new View("register",
                    new LoginRegisterResultDto { ErrorFieldName = "email", ErrorMsg = "Недопустимый формат у email" });
            if (password != repeat_password)
                return new View("register",
                    new LoginRegisterResultDto { ErrorFieldName = "repeat_password", ErrorMsg = "Повторный пароль несовпал с исходным" });

            var account = await AccountsData.GetAccountByLogin((Model.Account)Activator.CreateInstance(typeof(Model.Account)), login);
            if (account is not null)
                return new View("register",
                    new LoginRegisterResultDto { ErrorFieldName = "login", ErrorMsg = "Пользователь с таким логином уже существует" });

            var salt = PasswordHashingManager.CreateSalt();
            var encryptedPassword = PasswordHashingManager.GetSHA256(password + salt);

            var insertAccount = new Model.Account 
            { 
                Login = login, 
                Password = encryptedPassword, 
                Email = email, 
                Salt = salt, 
                TractorName = tractorName,
                DateRegistration = DateTime.Now,
            };
            await MyORM.Instance.Insert(insertAccount);
            return new Redirect("/login");
        }
    }

}
