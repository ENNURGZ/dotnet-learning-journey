using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MiddlewareLibrary.Middleware;

/// <summary>
/// Middleware that applies behavior based on configurable options before invoking the next component.
/// </summary>
public class ConfigurableMiddleware(RequestDelegate next, ConfigurableMiddlewareOptions options)
{
    /// <summary>
    /// Executes middleware behavior based on the provided configuration and invokes the next component in the pipeline.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        if (options.EnableLogging)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<ConfigurableMiddleware>>();
            logger.LogInformation("Processing request for path: {Path}", context.Request.Path);
        }

        if (options.EnableCustomHeader)
        {
            context.Response.Headers["X-Custom-Header"] = options.CustomHeaderValue;
        }

        await next(context);
    }
}

/// <summary>
/// Configuration options for <see cref="ConfigurableMiddleware"/>.
/// </summary>
public class ConfigurableMiddlewareOptions
{
    public bool EnableLogging { get; set; } = true;
    public bool EnableCustomHeader { get; set; }
    public string CustomHeaderValue { get; set; } = "Custom Value";
}

