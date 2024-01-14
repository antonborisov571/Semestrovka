using Framework.Server.CookieAndSession;
using Framework.Server.Responses;
using Framework.Server.Routing;
using Framework.Server.Routing.Attributes;
using ORM.ORMPostgres;
using Server.Extensions;
using Server.Model;

namespace Server.Controller
{
    [Controller("topic")]
    public class TopicController
    {
        [HttpGET("/{topicId}")]
        public async Task<IControllerResult> GetTopicAsync(int topicId)
        {
            var orm = MyORM.Instance;
            var topics = await orm.Select<Topic>();
            var accounts = await orm.Select<Account>();
            var posts = await orm.Select<Post>();

            var topic = topics.FirstOrDefault(x => x.Id == topicId);
            if (topic is null)
                return new NotFound();

            var preparedPosts = posts
                .Where(x => x.TopicId == topicId)
                .Select(x => 
                new 
                { 
                    User = accounts.FirstOrDefault(y => y.Id == x.UserId), 
                    PostInfo = x 
                });

            return new View("posts",
                new
                {
                    Topic = topic,
                    CountPosts = preparedPosts.Count(),
                    Posts = preparedPosts
                });
        }

        [HttpGET("/{topicId}/addpost")]
        public async Task<IControllerResult> PageAddPost(int topicId, [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
        {
            if (sessionId == default)
            {
                return new Redirect("/register");
            }

            var topics = await MyORM.Instance.Select<Topic>();
            var topic = topics.FirstOrDefault(x => x.Id == topicId);
            return new View("addpost", topic);
        }

        [HttpPOST("/{topicId}/addpost")]
        public async Task<IControllerResult> AddPost(int topicId, string postName,
            string postDescription,
            [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
        {
            if (string.IsNullOrEmpty(postName.Trim()))
                return new BadRequest();
            var sessionManager = SessionManager.Instance;
            int? userId = null;
            if (sessionId != default)
            {
                var session = await sessionManager.GetSession(sessionId);
                userId = session.AccountId;
            }
            var dateDispatch = DateTime.Now;
            await MyORM.Instance.Insert(new Post
            {
                UserId = (int)userId!,
                TopicId = topicId,
                PostName = postName,
                DateDispatch = dateDispatch,
                Description = postDescription
            });
            return new Redirect($"/topic/{topicId}");
        }

        [HttpGET("/{topicId}/messages/{postId}")]
        public async Task<IControllerResult> GetMessagesAsync(int topicId, int postId,
            [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId)
        {
            var orm = MyORM.Instance;
            var accounts = await orm.Select<Account>();
            var posts = await orm.Select<Post>();
            var topics = await orm.Select<Topic>();
            var messages = await orm.Select<Message>();
            var images = await orm.Select<Image>();
            var favourites = (IEnumerable<Favourite>?)new List<Favourite>();
            var likes = (IEnumerable<Like>?)new List<Like>();

            var topic = topics.FirstOrDefault(x => x.Id == topicId);
            var post = posts.FirstOrDefault(x => x.Id == postId);
            if (post is null || topic is null)
                return new NotFound();

            var sessionInfo = sessionId.ToString();
            int currentUserId = 0;
            if (sessionId != default)
            {
                var session = await SessionManager.Instance.GetSession(sessionId);
                if (session is not null)
                {
                    currentUserId = session.AccountId;
                    favourites = (await MyORM.Instance.Select<Favourite>()).Where(x => x.UserId == session.AccountId);
                    likes = (await MyORM.Instance.Select<Like>()).Where(x => x.UserId == session.AccountId);
                }
            }
            else
            {
                sessionInfo = "";
            }
            var preparedMessages = messages
                .Where(x => x.PostId == postId)
                .Select(x =>
                new
                {
                    User = accounts.FirstOrDefault(y => y.Id == x.UserId),
                    MessageInfo = x,
                    HasFavourite = favourites.Any(y => y.MessageId == x.Id && y.HasFavourite),
                    HasLike = likes.Any(y => y.MessageId == x.Id && y.HasLike),
                    IsOnline = accounts.FirstOrDefault(y => y.Id == x.UserId).LastOnline.AddMinutes(5) > DateTime.Now,
                    Images = images.Where(y => y.MessageId == x.Id)
                }).OrderBy(x => x.MessageInfo.DateDispatch);
            
            return new View("messages",
                new
                {
                    Post = post,
                    CountMessages = preparedMessages.Count(),
                    Messages = preparedMessages,
                    SessionInfo = sessionInfo, 
                    CurrentUserId = currentUserId
                });
        }

        [HttpPOST("/{topicId}/messages/{postId}")]
        public async Task<IControllerResult> SetMessageAsync(int topicId, int postId, string textMessage,
            [FromCookie(typeof(SessionId), nameof(SessionId.Id))] Guid sessionId,
            Context context)
        {
            if (string.IsNullOrEmpty(textMessage.Trim()))
                return new BadRequest();
            var sessionManager = SessionManager.Instance;
            int? userId = null;
            if (sessionId != default)
            {
                var session = await sessionManager.GetSession(sessionId);
                userId = session.AccountId;
            }
            var dateDispatch = DateTime.Now;
            var message = new Message
            {
                UserId = (int)userId!,
                PostId = postId,
                DateDispatch = dateDispatch,
                TextMessage = textMessage,
            };
            await MyORM.Instance.Insert(message);
            var messageNew = await MyORM.Instance.Select(message);
            var messageId = messageNew.FirstOrDefault().Id;
            return new JsonResult(messageId);
        }
    }
}
