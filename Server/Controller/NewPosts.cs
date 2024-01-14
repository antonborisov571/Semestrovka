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

[Controller("newposts")]
class NewPosts
{
    [HttpGET("/")]
    public async Task<IControllerResult> GetNewPosts()
    {
        var accounts = await MyORM.Instance.Select<Model.Account>();
        var posts = (await MyORM.Instance.Select<Post>())
            .OrderByDescending(x => x.DateDispatch)
            .Take(10)
            .Select(x => new
            {
                PostInfo = x,
                User = accounts.FirstOrDefault(y => y.Id == x.UserId)
            });
        return new View("newposts", posts);
    }
}
