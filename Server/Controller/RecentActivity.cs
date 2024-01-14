using Framework.Server.Responses;
using Framework.Server.Routing.Attributes;
using ORM.ORMPostgres;
using Server.Model;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controller;

[Controller()]
public class RecentActivity
{
    [HttpGET("/")]
    public async Task<IControllerResult> GetRecentActivity()
    {
        var likes = (await MyORM.Instance.Select<Like>()).OrderByDescending(x => x.DateDispatch).Take(20);
        var messages = (await MyORM.Instance.Select<Message>()).OrderByDescending(x => x.DateDispatch).Take(20);
        var posts = await MyORM.Instance.Select<Post>();
        var accounts = await MyORM.Instance.Select<Account>();

        List<dynamic> resultTemp = new List<dynamic>();
        foreach (var item in likes)
        {
            resultTemp.Add(item);
        }
        foreach (var item in messages)
        {
            resultTemp.Add(item);
        }
        var result = resultTemp.Select(x => new
        {
            Like = x,
            Message = x,
            IsLike = x is Like,
            IsMessage = x is Message,
            User = accounts.FirstOrDefault(y => y.Id == x.UserId),
            Liker = accounts.FirstOrDefault(y => x is Like && y.Id == x.UserId),
            Sender = accounts.FirstOrDefault(y => x is Like && messages.Any(z => x.MessageId == z.Id && z.UserId == y.Id)),
            Post = posts.FirstOrDefault(y => (x is Message && y.Id == x.PostId) 
                || (x is Like && messages.FirstOrDefault(z => z.Id == x.MessageId)?.PostId == y.Id)),
            MessageSend = messages.FirstOrDefault(y => x is Like && x.MessageId == y.Id),
            DateDispatch = x is Like ? ((Like)x).DateDispatch : ((Message)x).DateDispatch
        });

        result = result.OrderByDescending(x => x.DateDispatch).ToList().Take(10);

        return new View("recentactivity", result);
    }
}
