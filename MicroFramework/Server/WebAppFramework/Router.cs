using System.Text;
using Framework.Server.WebAppFramework.Abstractions;
using Framework.Server.WebAppFramework.Extensions;

namespace Framework.Server.WebAppFramework;

class Router
{
    private Dictionary<string, Dictionary<string, RequestDelegate>> routes;
    private PipelineMiddleWare Pipeline = new PipelineMiddleWare();

    public Router()
    {
        routes = new Dictionary<string, Dictionary<string, RequestDelegate>>();
        foreach (var method in new[] { "get", "post", "put", "delete" })
        {
            GetType().GetMethod(method)?.Invoke(this, new object[] { });
        }
    }

    public void Get(string path, params IMiddleWare[] middleware)
    {
        var fns = Pipeline.Compose(middleware.ToList());
        if (!routes.ContainsKey(path))
        {
            routes[path] = new Dictionary<string, RequestDelegate>();
        }
        routes[path]["get"] = fns;
    }

    public void Post(string path, params IMiddleWare[] middleware)
    {
        var fns = Pipeline.Compose(middleware.ToList());
        if (!routes.ContainsKey(path))
        {
            routes[path] = new Dictionary<string, RequestDelegate>();
        }
        routes[path]["post"] = fns;
    }

    public void Put(string path, params IMiddleWare[] middleware)
    {
        var fns = Pipeline.Compose(middleware.ToList());
        if (!routes.ContainsKey(path))
        {
            routes[path] = new Dictionary<string, RequestDelegate>();
        }
        routes[path]["put"] = fns;
    }

    public void Delete(string path, params IMiddleWare[] middleware)
    {
        var fns = Pipeline.Compose(middleware.ToList());
        if (!routes.ContainsKey(path))
        {
            routes[path] = new Dictionary<string, RequestDelegate>();
        }
        routes[path]["delete"] = fns;
    }

    public RequestDelegate ToMiddleware()
    {
        return async (ctx) =>
        {
            var req = ctx.Request;
            var res = ctx.Response;
            var path = req.Url?.LocalPath!;
            var method = req.HttpMethod.ToLower();

            if (!routes.ContainsKey(path))
            {
                await res.WriteBody("Данный путь не найден!", "application/json");
                res.StatusCode = 404;
                return;
            }

            if (!routes[path].ContainsKey(method))
            {
                await res.WriteBody("Данный метод не найден!", "application/json");
                res.StatusCode = 405;
                return;
            }

            await routes[path][method](ctx);
        };
    }
}
