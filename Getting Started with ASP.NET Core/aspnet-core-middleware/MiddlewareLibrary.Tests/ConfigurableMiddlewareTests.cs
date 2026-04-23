using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MiddlewareLibrary.Middleware;

namespace MiddlewareLibrary.Tests;

public class ConfigurableMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_AddsCustomHeader_WhenEnabled()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        var provider = services.BuildServiceProvider();

        var context = TestHelpers.CreateContext("/info");
        context.RequestServices = provider;

        var options = new ConfigurableMiddlewareOptions
        {
            EnableLogging = false,
            EnableCustomHeader = true,
            CustomHeaderValue = "My custom header"
        };

        var middleware = new ConfigurableMiddleware(_ => Task.CompletedTask, options);

        await middleware.InvokeAsync(context);

        Assert.Equal("My custom header", context.Response.Headers["X-Custom-Header"].ToString());
    }

    [Fact]
    public async Task InvokeAsync_DoesNotAddHeader_WhenDisabled()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        var provider = services.BuildServiceProvider();

        var context = TestHelpers.CreateContext("/info");
        context.RequestServices = provider;

        var options = new ConfigurableMiddlewareOptions
        {
            EnableLogging = true,
            EnableCustomHeader = false,
        };

        var middleware = new ConfigurableMiddleware(_ => Task.CompletedTask, options);

        await middleware.InvokeAsync(context);

        Assert.False(context.Response.Headers.ContainsKey("X-Custom-Header"));
    }

    [Fact]
    public async Task InvokeAsync_LogsPath_WhenLoggingEnabled()
    {
        var services = new ServiceCollection();
        var logger = new TestLogger<ConfigurableMiddleware>();
        services.AddSingleton(logger);
        services.AddSingleton<ILogger<ConfigurableMiddleware>>(logger);
        var provider = services.BuildServiceProvider();

        var context = TestHelpers.CreateContext("/loggable");
        context.RequestServices = provider;

        var options = new ConfigurableMiddlewareOptions
        {
            EnableLogging = true,
            EnableCustomHeader = false
        };

        var middleware = new ConfigurableMiddleware(_ => Task.CompletedTask, options);

        await middleware.InvokeAsync(context);

        Assert.Contains(logger.Entries, entry => entry.Message.Contains("/loggable"));
    }
}

