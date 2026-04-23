using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace MiddlewareLibrary.Tests;

internal static class TestHelpers
{
    public static DefaultHttpContext CreateContext(string path, string method = "GET")
    {
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            },
            Request =
            {
                Path = path,
                Method = method
            }
        };

        return context;
    }

    public static async Task<string> ReadResponseBodyAsync(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(response.Body, Encoding.UTF8, leaveOpen: true);
        var content = await reader.ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return content;
    }
}

