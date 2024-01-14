using Framework.AccountData;
using Framework.Server.CookieAndSession;
using Framework.Server.Responses;
using Framework.Server.Routing.Attributes;
using ORM.ORMPostgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controller;

[Controller]
public class Users
{
    [HttpGET("/{userId}")]
    public async Task<IControllerResult> ProfileViewerAsync(int userId)
    {
        var user = (await MyORM.Instance.Select<Model.Account>()).FirstOrDefault(x => x.Id == userId);
        return new View("usersProfile", user);
    }
}

