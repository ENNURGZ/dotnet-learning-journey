using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MiddlewareLibrary.Middleware;

/// <summary>
/// Middleware that enforces request rate limits based on client IP address.
/// </summary>
public class RateLimitingMiddleware(
    RequestDelegate next,
    ILogger<RateLimitingMiddleware> logger,
    RateLimitingOptions options)
{
    /// <summary>
    /// Applies rate limiting rules to the incoming request and forwards allowed requests to the next middleware.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    private static readonly ConcurrentDictionary<string, RequestLog> ClientRequests = new();

    public async Task InvokeAsync(HttpContext context)
    {
        if (ShouldSkip(context.Request.Path))
        {
            await next(context);
            return;
        }

        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        if (!IsRequestAllowed(clientIp))
        {
            logger.LogWarning("Rate limit exceeded for client: {ClientIp}", clientIp);
            await ReturnRateLimitExceeded(context);
            return;
        }

        await next(context);
    }

    private bool ShouldSkip(PathString path)
    {
        return options.ExcludedPaths.Any(p => path.StartsWithSegments(p));
    }

    private bool IsRequestAllowed(string clientIp)
    {
        var now = DateTime.UtcNow;
        var windowStart = now.AddMinutes(-options.WindowMinutes);

        var log = ClientRequests.GetOrAdd(clientIp, _ => new RequestLog());

        lock (log.Lock)
        {
            log.Timestamps.RemoveAll(t => t < windowStart);

            if (log.Timestamps.Count >= options.MaxRequests)
            {
                return false;
            }

            log.Timestamps.Add(now);
            return true;
        }
    }

    private static async Task ReturnRateLimitExceeded(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.Response.Headers.RetryAfter = "60";
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new { error = "Rate limit exceeded", retryAfter = 60 });
    }
}

internal class RequestLog
{
    public List<DateTime> Timestamps { get; } = new();
    public object Lock { get; } = new();
}

/// <summary>
/// Configuration options for <see cref="RateLimitingMiddleware"/>.
/// </summary>
public class RateLimitingOptions
{
    public int MaxRequests { get; set; } = 100;
    public int WindowMinutes { get; set; } = 1;
    public string[] ExcludedPaths { get; set; } = Array.Empty<string>();
}

