using System;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MiddlewareLibrary.Middleware;

/// <summary>
/// Middleware that compresses responses based on the client's accepted encodings.
/// </summary>
public class CompressionMiddleware(RequestDelegate next, ILogger<CompressionMiddleware> logger)
{
    /// <summary>
    /// Compresses the response stream when the client supports compression and forwards the request to the next middleware.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;
        var acceptEncoding = context.Request.Headers.AcceptEncoding.ToString();
        Stream? compressionStream = null;

        if (acceptEncoding.Contains("gzip", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.Headers.ContentEncoding = "gzip";
            compressionStream = new GZipStream(originalBodyStream, CompressionLevel.Fastest, leaveOpen: true);
        }
        else if (acceptEncoding.Contains("deflate", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.Headers.ContentEncoding = "deflate";
            compressionStream = new DeflateStream(originalBodyStream, CompressionLevel.Fastest, leaveOpen: true);
        }

        if (compressionStream != null)
        {
            context.Response.Body = compressionStream;
        }

        try
        {
            await next(context);
        }
        finally
        {
            if (compressionStream != null)
            {
                await compressionStream.DisposeAsync();
                context.Response.Body = originalBodyStream;
                logger.LogDebug("Response stream compressed for {Path}", context.Request.Path);
            }
        }
    }
}

