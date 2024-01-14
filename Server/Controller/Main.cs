using Framework.AccountData;
using Framework.Server.Responses;
using Framework.Server.Routing.Attributes;
using ORM.ORMPostgres;
using Server.Extensions;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Server.Controller
{
    [Controller("/")]
    public class Main
    {
        [HttpGET("/")]
        public async Task<IControllerResult> MainFooAsync()
        {
            var orm = MyORM.Instance;
            var posts = await orm.Select<Post>();
            var accounts = await orm.Select<Model.Account>();
            var joke = (await orm.Select<Joke>()).FirstOrDefault(x => x.DateShow.ToShortDateString() == DateTime.Now.ToShortDateString());
            return new View("main", 
                new 
                {
                    Joke = joke,
                    CountUsers = accounts.Count(),
                    CountPosts = posts.Count(),
                });
        }
    }
}
