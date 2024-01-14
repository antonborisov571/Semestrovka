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

namespace Server.Controller;

[Controller("addimagemessage")]
class AddImageMessageController
{
    [HttpPOST("/{messageId}")]
    public async Task<IControllerResult> AddImage(int messageId, string content)
    {
        await MyORM.Instance.Insert(new Image
        {
            MessageId = messageId,
            UrlImage = content
        });
        return new Ok();
    }
}