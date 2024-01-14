using System.Net;
using System.Net.Http;
using Framework.Server.WebAppFramework.Abstractions;
using Framework.Server.WebAppFramework.Extensions;
using Framework.Server.WebAppFramework.MiddleWares;

namespace Framework.Server.WebAppFramework;


public class WebApplication : IWebApplicationBuilder
{
    private List<IMiddleWare> _middleWares;

    private HttpListener _listener;

    private Router _router;


    public WebApplication()
    {
        _middleWares = new List<IMiddleWare>();
        _router = new Router();
        _listener = new HttpListener();
    }

    public IWebApplicationBuilder UseMiddleWare<TMiddleWare>(TMiddleWare MiddleWare) where TMiddleWare : IMiddleWare
    {
        _middleWares.Add(MiddleWare);
        return this;
    }

    public IWebApplicationBuilder UseMiddleWare<TMiddleWare>() where TMiddleWare : IMiddleWare, new()
    {
        _middleWares.Add(new TMiddleWare());
        return this;
    }

    public IWebApplicationBuilder UseMiddleWare(RequestDelegate requestDelegate)
    {
        _middleWares.Add(new MiddleWare(requestDelegate));
        return this;
    }

    public AttributeRequestsHandler UseControllers()
    {
        AttributeRequestsHandler _handler = new();
        return _handler;
    }

    public IWebApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
    {
        RequestDelegate request = (context) =>
        {
            return Task.CompletedTask;
        };
        _middleWares.Add(new MiddleWare(middleware(request)));
        return this;
    }

    public void Listen(params string[] prefixes)
    {
        foreach (string prefix in prefixes) 
        {
            _listener.Prefixes.Add(prefix);
        }

        _listener.Start();

        Console.WriteLine($"Прослушивание по адресу: {prefixes[0]}"); 
    }

    public void Run()
    {
        if (_listener == null)
            throw new InvalidOperationException("Выполни сначала метод Listen.");

        while (_listener.IsListening)
        {
            var context = _listener.GetContext();
            Task.Run(() => HandleRequest(context));
        }
    }

    private async Task HandleRequest(HttpListenerContext context)
    {
        var pipeline = new PipelineMiddleWare();
        var functions = pipeline.Compose(_middleWares);
        await functions(context);
    }

    public static WebApplicationBuilder CreateBuilder()
    {
        return new WebApplicationBuilder();
    }

    public void MapRoute(string route, string method, RequestDelegate middleware)
    {
        switch (method)
        {
            case "get":
                _router.Get(route, new MiddleWare(middleware));
                break;
            case "post":
                _router.Post(route, new MiddleWare(middleware));
                break;
            case "put":
                _router.Put(route, new MiddleWare(middleware));
                break;
            case "delete":
                _router.Delete(route, new MiddleWare(middleware));
                break;
            default:
                throw new InvalidOperationException("Данный http-метод не поддерживается");
        }
        UseMiddleWare(_router.ToMiddleware());
    }
}
