using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Server.WebAppFramework.Abstractions;

public interface IWebApplicationBuilder
{
    IWebApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware);

}
