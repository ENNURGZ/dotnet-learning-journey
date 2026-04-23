using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using MiddlewareLibrary.Middleware;

namespace MiddlewareLibrary.Tests;

public class AuthenticationMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_SkipsAuthentication_ForExcludedPath()
    {
        var called = false;
        RequestDelegate next = _ =>
        {
            called = true;
            return Task.CompletedTask;
        };

        var middleware = CreateMiddleware(next);
        var context = TestHelpers.CreateContext("/");

        await middleware.InvokeAsync(context);

        Assert.True(called);
        Assert.NotEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
    }

    [Theory]
    [InlineData("/health/status")]
    [InlineData("/swagger/index.html")]
    public async Task InvokeAsync_SkipsAuthentication_ForWhitelistedPaths(string path)
    {
        var called = false;
        RequestDelegate next = _ =>
        {
            called = true;
            return Task.CompletedTask;
        };

        var middleware = CreateMiddleware(next);
        var context = TestHelpers.CreateContext(path);

        await middleware.InvokeAsync(context);

        Assert.True(called);
        Assert.NotEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ReturnsUnauthorized_WhenApiKeyMissing()
    {
        var middleware = CreateMiddleware(_ => Task.CompletedTask);
        var context = TestHelpers.CreateContext("/protected");

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        var body = await TestHelpers.ReadResponseBodyAsync(context.Response);
        Assert.Contains("API key not provided", body);
    }

    [Fact]
    public async Task InvokeAsync_ReturnsUnauthorized_WhenApiKeyInvalid()
    {
        var middleware = CreateMiddleware(_ => Task.CompletedTask);
        var context = TestHelpers.CreateContext("/protected");
        context.Request.Headers["X-API-Key"] = "wrong";

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        var body = await TestHelpers.ReadResponseBodyAsync(context.Response);
        Assert.Contains("Invalid API key", body);
    }

    [Fact]
    public async Task InvokeAsync_ReturnsUnauthorized_WhenApiKeyNotConfigured()
    {
        var called = false;
        RequestDelegate next = _ =>
        {
            called = true;
            return Task.CompletedTask;
        };

        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>()).Build();
        var middleware = CreateMiddleware(next, configuration);
        var context = TestHelpers.CreateContext("/protected");
        context.Request.Headers["X-API-Key"] = "any";

        await middleware.InvokeAsync(context);

        Assert.False(called);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        var body = await TestHelpers.ReadResponseBodyAsync(context.Response);
        Assert.Contains("Invalid API key", body);
    }

    [Fact]
    public async Task InvokeAsync_CallsNext_WhenApiKeyValid()
    {
        var called = false;
        RequestDelegate next = _ =>
        {
            called = true;
            return Task.CompletedTask;
        };

        var middleware = CreateMiddleware(next);
        var context = TestHelpers.CreateContext("/protected");
        context.Request.Headers["X-API-Key"] = "secret";

        await middleware.InvokeAsync(context);

        Assert.True(called);
        Assert.NotEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
    }

    private static AuthenticationMiddleware CreateMiddleware(RequestDelegate next, IConfiguration? configuration = null)
    {
        configuration ??= new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ApiKey"] = "secret"
            })
            .Build();

        return new AuthenticationMiddleware(next, NullLogger<AuthenticationMiddleware>.Instance, configuration);
    }
}

