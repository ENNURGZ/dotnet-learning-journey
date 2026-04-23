using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MiddlewareLibrary.Middleware;

/// <summary>
/// Middleware that validates incoming requests using an API key before allowing access.
/// </summary>
public class AuthenticationMiddleware(
    RequestDelegate next,
    ILogger<AuthenticationMiddleware> logger,
    IConfiguration configuration)
{
    /// <summary>
    /// Validates the incoming request using an API key and invokes the next middleware when authentication succeeds.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        if (ShouldSkipAuthentication(context.Request.Path))
        {
            await next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-API-Key", out var providedApiKey))
        {
            logger.LogWarning("API key not provided for request {Path}", context.Request.Path);
            await ReturnUnauthorized(context, "API key not provided");
            return;
        }

        var apiKey = configuration["ApiKey"];
        if (providedApiKey != apiKey)
        {
            logger.LogWarning("Invalid API key provided for request {Path}", context.Request.Path);
            await ReturnUnauthorized(context, "Invalid API key");
            return;
        }

        logger.LogInformation("Authentication successful for request {Path}", context.Request.Path);
        await next(context);
    }

    private static bool ShouldSkipAuthentication(PathString path)
    {
        return path == "/" ||
               path.StartsWithSegments("/health") ||
               path.StartsWithSegments("/swagger");
    }

    private static async Task ReturnUnauthorized(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            error = "Authentication error",
            message,
            timestamp = DateTime.UtcNow
        });
    }
}

