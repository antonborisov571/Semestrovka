using Framework.Server.Routing;

namespace Framework.Server.Responses;

public class ComposeResult : IControllerResult
{
    IControllerResult[] Results { get; set; }

    public ComposeResult(params IControllerResult[] results)
    {
        Results = results;
    }

    public void ExecuteResult(Context context)
    {
        foreach (var result in Results)
        {
            result.ExecuteResult(context);
        }
    }
}
