using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MiddlewareLibrary.Extensions;
using MiddlewareLibrary.Middleware;

namespace MiddlewareLibrary.Tests;

public class MiddlewareExtensionsTests
{
    [Fact]
    public async Task UseConfigurableMiddleware_AppliesConfiguredOptions()
    {
        var services = new ServiceCollection();
        var logger = new TestLogger<ConfigurableMiddleware>();
        services.AddSingleton<ILogger<ConfigurableMiddleware>>(logger);
        var provider = services.BuildServiceProvider();

        var builder = new ApplicationBuilder(provider);
        builder.UseConfigurableMiddleware(options =>
        {
            options.EnableLogging = false;
            options.EnableCustomHeader = true;
            options.CustomHeaderValue = "configured";
        });
        builder.Run(context => context.Response.WriteAsync("ok"));

        var app = builder.Build();
        var context = TestHelpers.CreateContext("/configurable");
        context.RequestServices = provider;

        await app(context);

        Assert.Equal("configured", context.Response.Headers["X-Custom-Header"]);
    }

    [Fact]
    public async Task UseCustomCaching_UsesProvidedOptions()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        var provider = services.BuildServiceProvider();

        var builder = new ApplicationBuilder(provider);
        var executionCount = 0;
        builder.UseCustomCaching(options =>
        {
            options.CacheDurationMinutes = 1;
            options.CacheablePaths = new[] { "/cached" };
        });
        builder.Run(context =>
        {
            executionCount++;
            return context.Response.WriteAsync($"count-{executionCount}");
        });

        var app = builder.Build();

        var firstContext = TestHelpers.CreateContext("/cached/resource");
        firstContext.RequestServices = provider;
        await app(firstContext);

        var secondContext = TestHelpers.CreateContext("/cached/resource");
        secondContext.RequestServices = provider;
        await app(secondContext);

        Assert.Equal(1, executionCount);
        var body = await TestHelpers.ReadResponseBodyAsync(secondContext.Response);
        Assert.Equal("count-1", body);
    }

    [Fact]
    public void UseRequestLogging_ThrowsArgumentNull_WhenBuilderNull()
    {
        Assert.Throws<ArgumentNullException>(() => MiddlewareExtensions.UseRequestLogging(null!));
    }
}


