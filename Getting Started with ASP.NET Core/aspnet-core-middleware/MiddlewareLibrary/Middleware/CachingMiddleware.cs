using System;
using System.Collections.Concurrent;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MiddlewareLibrary.Middleware;

/// <summary>
/// Middleware that caches responses for configured paths to improve performance.
/// </summary>
public class CachingMiddleware(RequestDelegate next, ILogger<CachingMiddleware> logger, CachingOptions options)
{
    /// <summary>
    /// Processes the incoming request with caching logic and serves a cached response when available.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    private static readonly ConcurrentDictionary<string, CacheEntry> Cache = new();

    public async Task InvokeAsync(HttpContext context)
    {
        if (!ShouldCache(context.Request.Path))
        {
            await next(context);
            return;
        }

        var cacheKey = $"{context.Request.Method}:{context.Request.Path}:{context.Request.QueryString}";

        if (TryGetFromCache(cacheKey, out var cachedEntry))
        {
            logger.LogInformation("Cache HIT for {Path}", context.Request.Path);
            await WriteCachedResponse(context, cachedEntry);
            return;
        }

        logger.LogInformation("Cache MISS for {Path}", context.Request.Path);

        var originalBodyStream = context.Response.Body;
        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        await next(context);

        if (context.Response.StatusCode == StatusCodes.Status200OK)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            var content = memoryStream.ToArray();

            var entry = new CacheEntry(
                content,
                context.Response.ContentType ?? string.Empty,
                context.Response.StatusCode,
                DateTime.UtcNow.AddMinutes(options.CacheDurationMinutes));

            SaveToCache(cacheKey, entry);
            AddCacheHeaders(context.Response.Headers);
        }

        memoryStream.Seek(0, SeekOrigin.Begin);
        await memoryStream.CopyToAsync(originalBodyStream);
        context.Response.Body = originalBodyStream;
    }

    private bool ShouldCache(PathString path)
    {
        return options.CacheablePaths.Any(p => path.StartsWithSegments(p));
    }

    private static bool TryGetFromCache(string key, out CacheEntry entry)
    {
        if (Cache.TryGetValue(key, out var cachedEntry))
        {
            if (cachedEntry.Expiry > DateTime.UtcNow)
            {
                entry = cachedEntry;
                return true;
            }

            Cache.TryRemove(key, out _);
        }

        entry = null!;
        return false;
    }

    private static void SaveToCache(string key, CacheEntry entry)
    {
        Cache[key] = entry;
    }

    private static void AddCacheHeaders(IHeaderDictionary headers)
    {
        headers.CacheControl = "public, max-age=3600";
        headers.ETag = $"\"{Guid.NewGuid():N}\"";
    }

    private static async Task WriteCachedResponse(HttpContext context, CacheEntry entry)
    {
        context.Response.StatusCode = entry.StatusCode;
        context.Response.ContentType = entry.ContentType;
        AddCacheHeaders(context.Response.Headers);
        await context.Response.Body.WriteAsync(entry.Content);
    }
}

public record CacheEntry(byte[] Content, string ContentType, int StatusCode, DateTime Expiry);

/// <summary>
/// Configuration options for <see cref="CachingMiddleware"/>.
/// </summary>
public class CachingOptions
{
    public int CacheDurationMinutes { get; set; } = 60;
    public string[] CacheablePaths { get; set; } = Array.Empty<string>();
}

