using Framework.Server.WebAppFramework.Abstractions;
using System.Net;

namespace Framework.Server.WebAppFramework.Extensions;

public static class UseExtensions
{
    public static IWebApplicationBuilder Use(
        this IWebApplicationBuilder app,
        Func<HttpListenerContext, Func<Task>, Task> middleware)
    {
        return app.Use(next =>
        {
            return context =>
            {
                Func<Task> simpleNext = () => next(context);
                return middleware(context, simpleNext);
            };
        });
    }
}
