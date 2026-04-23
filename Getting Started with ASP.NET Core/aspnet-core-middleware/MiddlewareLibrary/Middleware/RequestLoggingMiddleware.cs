using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MiddlewareLibrary.Middleware;

/// <summary>
/// Middleware that logs request and response metadata throughout the request lifecycle.
/// </summary>
public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    /// <summary>
    /// Logs request and response metadata and passes control to the next middleware in the pipeline.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;
        var requestId = Guid.NewGuid().ToString("N")[..8];

        context.Response.Headers["X-Request-ID"] = requestId;

        logger.LogInformation("[{RequestId}] Starting request processing: {Method} {Path}", requestId, context.Request.Method, context.Request.Path);

        await next(context);

        var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;

        logger.LogInformation("[{RequestId}] Request processing completed: {StatusCode} in {Duration}ms", requestId, context.Response.StatusCode, duration);
    }
}

