using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Server.WebAppFramework.Extensions;

public static class StreamExtensions
{
    public static async Task WriteAsync(this Stream stream, string content)
    {
        await stream.WriteAsync(Encoding.UTF8.GetBytes(content)).AsTask();
    }
}
