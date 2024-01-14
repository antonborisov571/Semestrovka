using Framework.Server.Responses;
using Framework.Server.Routing.Attributes;
using ORM.ORMPostgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controller;

[Controller()]
class TopUsers
{
    [HttpGET("/")]
    public async Task<IControllerResult> GetTopUsers()
    {
        var accounts = (await MyORM.Instance.Select<Model.Account>()).OrderByDescending(x => x.Reputation).Take(100);
        return new View("topusers", accounts);
    }
}
