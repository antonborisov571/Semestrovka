using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Framework.Server.WebAppFramework.Abstractions;

namespace Framework.Server.WebAppFramework.MiddleWares;

class MiddleWare : IMiddleWare
{
    public RequestDelegate? Next { get; set; }

    private RequestDelegate _delegate { get; set; }

    public Task Invoke(HttpListenerContext context) => _delegate.Invoke(context);

    public MiddleWare(RequestDelegate @delegate)
    {
        _delegate = @delegate;
    }

    public MiddleWare(RequestDelegate @delegate, RequestDelegate next)
    {
        _delegate = @delegate;
        Next = next;
    }
}
