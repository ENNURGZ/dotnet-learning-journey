using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using MiddlewareLibrary.Middleware;

namespace MiddlewareLibrary.Tests;

public class RateLimitingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_BlocksExcessiveRequests()
    {
        var options = new RateLimitingOptions
        {
            MaxRequests = 2,
            WindowMinutes = 1,
            ExcludedPaths = Array.Empty<string>()
        };

        var middleware = new RateLimitingMiddleware(_ => Task.CompletedTask, NullLogger<RateLimitingMiddleware>.Instance, options);

        var firstContext = TestHelpers.CreateContext("/api/data");
        firstContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
        await middleware.InvokeAsync(firstContext);

        var secondContext = TestHelpers.CreateContext("/api/data");
        secondContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
        await middleware.InvokeAsync(secondContext);

        var thirdContext = TestHelpers.CreateContext("/api/data");
        thirdContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
        await middleware.InvokeAsync(thirdContext);

        Assert.Equal(StatusCodes.Status429TooManyRequests, thirdContext.Response.StatusCode);
        Assert.Equal("60", thirdContext.Response.Headers["Retry-After"].ToString());
    }

    [Fact]
    public async Task InvokeAsync_SkipsExcludedPaths()
    {
        var options = new RateLimitingOptions
        {
            MaxRequests = 1,
            WindowMinutes = 1,
            ExcludedPaths = new[] { "/health" }
        };

        var callCount = 0;
        RequestDelegate next = _ =>
        {
            callCount++;
            return Task.CompletedTask;
        };

        var middleware = new RateLimitingMiddleware(next, NullLogger<RateLimitingMiddleware>.Instance, options);

        for (var i = 0; i < 3; i++)
        {
            var context = TestHelpers.CreateContext("/health/status");
            context.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.2");
            await middleware.InvokeAsync(context);

            Assert.NotEqual(StatusCodes.Status429TooManyRequests, context.Response.StatusCode);
        }

        Assert.Equal(3, callCount);
    }

    [Fact]
    public async Task InvokeAsync_ReturnsJsonPayload_WhenLimitExceeded()
    {
        var options = new RateLimitingOptions
        {
            MaxRequests = 1,
            WindowMinutes = 1
        };

        var middleware = new RateLimitingMiddleware(_ => Task.CompletedTask, NullLogger<RateLimitingMiddleware>.Instance, options);

        var firstContext = TestHelpers.CreateContext("/api/data");
        firstContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.10");
        await middleware.InvokeAsync(firstContext);

        var secondContext = TestHelpers.CreateContext("/api/data");
        secondContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.10");
        await middleware.InvokeAsync(secondContext);

        Assert.Equal(StatusCodes.Status429TooManyRequests, secondContext.Response.StatusCode);
        var body = await TestHelpers.ReadResponseBodyAsync(secondContext.Response);
        Assert.Contains("Rate limit exceeded", body);
        Assert.Contains("\"retryAfter\":60", body);
    }
}

