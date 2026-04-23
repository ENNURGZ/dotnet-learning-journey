using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using MiddlewareLibrary.Middleware;

namespace MiddlewareLibrary.Tests;

public class RequestLoggingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_AddsRequestIdHeader()
    {
        var context = TestHelpers.CreateContext("/test");
        var middleware = new RequestLoggingMiddleware(_ => Task.CompletedTask, NullLogger<RequestLoggingMiddleware>.Instance);

        await middleware.InvokeAsync(context);

        Assert.True(context.Response.Headers.ContainsKey("X-Request-ID"));
    }

    [Fact]
    public async Task InvokeAsync_WritesStartAndCompletionLog()
    {
        var logger = new TestLogger<RequestLoggingMiddleware>();
        RequestDelegate next = async context =>
        {
            await context.Response.WriteAsync("ok");
        };

        var middleware = new RequestLoggingMiddleware(next, logger);
        var context = TestHelpers.CreateContext("/log");

        await middleware.InvokeAsync(context);

        var messages = logger.Entries.Select(entry => entry.Message).ToList();
        Assert.Equal(2, messages.Count);
        Assert.Contains(messages, message => message.Contains("Starting request processing"));
        Assert.Contains(messages, message => message.Contains("Request processing completed"));
    }
}

