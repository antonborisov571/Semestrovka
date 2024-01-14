using Framework.AccountData;
using Framework.Server.CookieAndSession;
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

namespace Server.Controller;

[Controller("contentmain")]
public class ContentMainLoader
{
    [HttpGET("/")]
    public async Task<IControllerResult> GetContentAsync()
    {
        var orm = MyORM.Instance;
        var topics = await orm.Select<Topic>();
        var posts = await orm.Select<Post>();
        var accounts = await orm.Select<Model.Account>();
        var resultTopics = topics
            .Select(x => new
            {
                User = accounts.FirstOrDefault(y => y.Id == x.UserId),
                TopicName = x.TopicName,
                Id = x.Id,
                Time = x.DateDispatch,
                UserId = x.UserId,
                Description = x.Description,
                CountPosts = posts.Count(y => y.TopicId == x.Id),
            }).OrderByDescending(x => x.Time);
        return new View("contentmain",
            new
            {
                Topics = resultTopics,
                CountUsers = accounts.Count(),
                CountPosts = topics.Count(),
            });
    }
}
