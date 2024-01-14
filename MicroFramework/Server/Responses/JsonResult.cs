using Framework.Server.Extensions;
using Framework.Server.Routing;
using System.Text;
using System.Text.Json;

namespace Framework.Server.Responses;

public class JsonResult : IControllerResult
{
    object data;
    public JsonResult(object data) => this.data = data;

    public void ExecuteResult(Context context)
    {
        context.HttpContext.Response.WriteBody(
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data)), 
            "application/json");
    }
}
