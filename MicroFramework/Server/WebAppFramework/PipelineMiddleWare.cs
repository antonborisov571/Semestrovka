using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Framework.Server.WebAppFramework.Abstractions;

namespace Framework.Server.WebAppFramework;

class PipelineMiddleWare
{
    public RequestDelegate? functions;

    public RequestDelegate Compose(List<IMiddleWare> middlewares)
    {
        functions += middlewares[0].Invoke;
        for (var i = 0; i < middlewares.Count - 1; i++)
        {
            if (middlewares[i].Next != null)
            {
                functions += middlewares[i].Next;
            }
            middlewares[i].Next = middlewares[i + 1].Invoke;
            functions += middlewares[i].Next;
        }
        return functions;
    }
}
