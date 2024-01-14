namespace Framework.Server.WebAppFramework;

public sealed class WebApplicationBuilder
{
    public WebApplication Build()
    {
        return new WebApplication();
    }

    public WebApplicationBuilder()
    {
    }
}
