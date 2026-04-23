using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using MiddlewareLibrary.Middleware;

namespace MiddlewareLibrary.Tests;

public class CachingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ReturnsCachedResponse_OnSecondCall()
    {
        var options = new CachingOptions
        {
            CacheDurationMinutes = 1,
            CacheablePaths = ["/static"]
        };

        var executionCount = 0;
        RequestDelegate next = async context =>
        {
            executionCount++;
            await context.Response.WriteAsync($"payload-{executionCount}");
        };

        var middleware = new CachingMiddleware(next, NullLogger<CachingMiddleware>.Instance, options);

        var firstContext = TestHelpers.CreateContext("/static/data");
        await middleware.InvokeAsync(firstContext);
        var firstResponse = await TestHelpers.ReadResponseBodyAsync(firstContext.Response);

        var secondContext = TestHelpers.CreateContext("/static/data");
        await middleware.InvokeAsync(secondContext);
        var secondResponse = await TestHelpers.ReadResponseBodyAsync(secondContext.Response);

        Assert.Equal(1, executionCount);
        Assert.Equal(firstResponse, secondResponse);
    }

    [Fact]
    public async Task InvokeAsync_DoesNotCache_WhenPathNotListed()
    {
        var options = new CachingOptions
        {
            CacheDurationMinutes = 1,
            CacheablePaths = new[] { "/static" }
        };

        var executionCount = 0;
        RequestDelegate next = async context =>
        {
            executionCount++;
            await context.Response.WriteAsync($"payload-{executionCount}");
        };

        var middleware = new CachingMiddleware(next, NullLogger<CachingMiddleware>.Instance, options);

        var firstContext = TestHelpers.CreateContext("/dynamic/data");
        await middleware.InvokeAsync(firstContext);

        var secondContext = TestHelpers.CreateContext("/dynamic/data");
        await middleware.InvokeAsync(secondContext);

        Assert.Equal(2, executionCount);
    }

    [Fact]
    public async Task InvokeAsync_SetsCacheHeaders_OnCacheHit()
    {
        var options = new CachingOptions
        {
            CacheDurationMinutes = 1,
            CacheablePaths = new[] { "/assets" }
        };

        RequestDelegate next = async context =>
        {
            await context.Response.WriteAsync("asset");
        };

        var middleware = new CachingMiddleware(next, NullLogger<CachingMiddleware>.Instance, options);

        var firstContext = TestHelpers.CreateContext("/assets/logo");
        await middleware.InvokeAsync(firstContext);

        var secondContext = TestHelpers.CreateContext("/assets/logo");
        await middleware.InvokeAsync(secondContext);

        Assert.True(secondContext.Response.Headers.ContainsKey("Cache-Control"));
        Assert.True(secondContext.Response.Headers.ContainsKey("ETag"));
    }

    [Fact]
    public async Task InvokeAsync_ExpiresImmediately_WhenDurationZero()
    {
        var options = new CachingOptions
        {
            CacheDurationMinutes = 0,
            CacheablePaths = new[] { "/instant" }
        };

        var executionCount = 0;
        RequestDelegate next = async context =>
        {
            executionCount++;
            await context.Response.WriteAsync($"instant-{executionCount}");
        };

        var middleware = new CachingMiddleware(next, NullLogger<CachingMiddleware>.Instance, options);

        var firstContext = TestHelpers.CreateContext("/instant/data");
        await middleware.InvokeAsync(firstContext);

        var secondContext = TestHelpers.CreateContext("/instant/data");
        await middleware.InvokeAsync(secondContext);

        Assert.Equal(2, executionCount);
    }
}

