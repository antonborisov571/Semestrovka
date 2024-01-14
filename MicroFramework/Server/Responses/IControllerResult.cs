using Framework.Server.Routing;

namespace Framework.Server.Responses;

public interface IControllerResult
{
    void ExecuteResult(Context context);
}
