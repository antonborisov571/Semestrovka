using Framework.Server.CookieAndSession;
using Framework.Server.Responses;
using Framework.Server.Routing.Attributes;
using ORM.ORMPostgres;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controller
{
    [Controller("addtopic")]
    public class AddTopicController
    {
        [HttpGET("/")]
        public IControllerResult PageAddTopic([FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
        {
            if (sessionId == default)
            {
                return new Redirect("/register");
            }
            return new View("addtopic");
        }

        [HttpPOST("/")]
        public async Task<IControllerResult> AddTopic(string topicName,
            string topicDescription,
            [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
        {
            if (string.IsNullOrEmpty(topicName.Trim()))
                return new BadRequest();
            var sessionManager = SessionManager.Instance;
            int? userId = null;
            if (sessionId != default)
            {
                var session = await sessionManager.GetSession(sessionId);
                userId = session.AccountId;
            }
            var dateDispatch = DateTime.Now;
            await MyORM.Instance.Insert(new Topic
            {
                UserId = userId,
                TopicName = topicName,
                DateDispatch = dateDispatch,
                Description = topicDescription
            });
            return new Redirect("/");
        }
    }
}
