using Framework.Server.Responses;
using Framework.Server.Routing.Attributes;
using ORM.ORMPostgres;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Server.Controller;

[Controller("find")]
public class FindController
{
    [HttpGET("/{selectedType}/{selectedTime}")]
    public async Task<IControllerResult> GetFind(string textfind, string selectedType, string selectedTime)
    {
        var textFind = HttpUtility.UrlDecode(textfind);
        var accounts = await MyORM.Instance.Select<Account>();
        var result = new
        {
            HasTopics = true,
            HasPosts = true,
            HasMessages = true,
            TextFind = textFind,
            Topics = (await MyORM.Instance.Select<Topic>())
            .Where(x => x.TopicName.Contains(textFind) || x.Description.Contains(textFind))
            .Select(x => new
            {
                TopicInfo = x,
                User = accounts.FirstOrDefault(y => y.Id == x.UserId)
            }),
            Posts = (await MyORM.Instance.Select<Post>())
            .Where(x => x.PostName.Contains(textFind) || x.Description.Contains(textFind))
            .Select(x => new
            {
                PostInfo = x,
                User = accounts.FirstOrDefault(y => y.Id == x.UserId)
            }),
            Messages = (await MyORM.Instance.Select<Message>())
            .Where(x => x.TextMessage.Contains(textFind))
            .Select(x => new
            {
                MessageInfo = x,
                User = accounts.FirstOrDefault(y => y.Id == x.UserId)
            }),
        };

        switch (selectedType)
        {
            case "all":
                break;
            case "topics":
                result = new
                {
                    HasTopics = true,
                    HasPosts = false,
                    HasMessages = false,
                    result.TextFind,
                    result.Topics,
                    result.Posts,
                    result.Messages,
                };
                break;
            case "posts":
                result = new
                {
                    HasTopics = false,
                    HasPosts = true,
                    HasMessages = false,
                    result.TextFind,
                    result.Topics,
                    result.Posts,
                    result.Messages,
                };
                break;
            case "messages":
                result = new
                {
                    HasTopics = false,
                    HasPosts = false,
                    HasMessages = true,
                    result.TextFind,
                    result.Topics,
                    result.Posts,
                    result.Messages,
                };
                break;
        }


        switch (selectedTime)
        {
            case "all":
                return new View("find", result);
            case "day":
                result = new
                {
                    result.HasTopics,
                    result.HasPosts,
                    result.HasMessages,
                    result.TextFind,
                    Topics = result.Topics.Where(x => x.TopicInfo.DateDispatch.AddDays(1) > DateTime.Now),
                    Posts = result.Posts.Where(x => x.PostInfo.DateDispatch.AddDays(1) > DateTime.Now),
                    Messages = result.Messages.Where(x => x.MessageInfo.DateDispatch.AddDays(1) > DateTime.Now),
                };
                return new View("find", result);
            case "week":
                result = new
                {
                    result.HasTopics,
                    result.HasPosts,
                    result.HasMessages,
                    result.TextFind,
                    Topics = result.Topics.Where(x => x.TopicInfo.DateDispatch.AddDays(7) > DateTime.Now),
                    Posts = result.Posts.Where(x => x.PostInfo.DateDispatch.AddDays(7) > DateTime.Now),
                    Messages = result.Messages.Where(x => x.MessageInfo.DateDispatch.AddDays(7) > DateTime.Now),
                };
                return new View("find", result);
            case "month":
                result = new
                {
                    result.HasTopics,
                    result.HasPosts,
                    result.HasMessages,
                    result.TextFind,
                    Topics = result.Topics.Where(x => x.TopicInfo.DateDispatch.AddMonths(1) > DateTime.Now),
                    Posts = result.Posts.Where(x => x.PostInfo.DateDispatch.AddMonths(1) > DateTime.Now),
                    Messages = result.Messages.Where(x => x.MessageInfo.DateDispatch.AddMonths(1) > DateTime.Now),
                };
                return new View("find", result);
        }
        return new View("/", result);
    }
}
