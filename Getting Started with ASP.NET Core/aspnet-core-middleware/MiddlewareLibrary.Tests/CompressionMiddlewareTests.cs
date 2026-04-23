using System.IO.Compression;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using MiddlewareLibrary.Middleware;

namespace MiddlewareLibrary.Tests;

public class CompressionMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_CompressesWithGzip_WhenClientSupportsGzip()
    {
        const string payload = "compressed-response";

        RequestDelegate next = async context =>
        {
            await context.Response.WriteAsync(payload);
        };

        var middleware = new CompressionMiddleware(next, NullLogger<CompressionMiddleware>.Instance);
        var context = TestHelpers.CreateContext("/gzip");
        context.Request.Headers.AcceptEncoding = "gzip";

        await middleware.InvokeAsync(context);

        Assert.Equal(new StringValues("gzip"), context.Response.Headers.ContentEncoding);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var gzipStream = new GZipStream(context.Response.Body, CompressionMode.Decompress, leaveOpen: true);
        using var reader = new StreamReader(gzipStream);
        var decompressed = await reader.ReadToEndAsync();

        Assert.Equal(payload, decompressed);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
    }

    [Fact]
    public async Task InvokeAsync_CompressesWithDeflate_WhenClientSupportsDeflate()
    {
        const string payload = "deflate-response";

        RequestDelegate next = async context =>
        {
            await context.Response.WriteAsync(payload);
        };

        var middleware = new CompressionMiddleware(next, NullLogger<CompressionMiddleware>.Instance);
        var context = TestHelpers.CreateContext("/deflate");
        context.Request.Headers.AcceptEncoding = "deflate";

        await middleware.InvokeAsync(context);

        Assert.Equal(new StringValues("deflate"), context.Response.Headers.ContentEncoding);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var deflateStream = new DeflateStream(context.Response.Body, CompressionMode.Decompress, leaveOpen: true);
        using var reader = new StreamReader(deflateStream);
        var decompressed = await reader.ReadToEndAsync();

        Assert.Equal(payload, decompressed);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
    }

    [Fact]
    public async Task InvokeAsync_DoesNotCompress_WhenEncodingNotSupported()
    {
        const string payload = "plain-response";

        RequestDelegate next = async context =>
        {
            await context.Response.WriteAsync(payload);
        };

        var middleware = new CompressionMiddleware(next, NullLogger<CompressionMiddleware>.Instance);
        var context = TestHelpers.CreateContext("/plain");
        context.Request.Headers.AcceptEncoding = "br";

        await middleware.InvokeAsync(context);

        Assert.True(StringValues.IsNullOrEmpty(context.Response.Headers.ContentEncoding));
        var body = await TestHelpers.ReadResponseBodyAsync(context.Response);
        Assert.Equal(payload, body);
    }

    [Fact]
    public async Task InvokeAsync_PrefersGzip_WhenMultipleEncodingsProvided()
    {
        const string payload = "multi-encoding";

        RequestDelegate next = async context =>
        {
            await context.Response.WriteAsync(payload);
        };

        var middleware = new CompressionMiddleware(next, NullLogger<CompressionMiddleware>.Instance);
        var context = TestHelpers.CreateContext("/multi");
        context.Request.Headers.AcceptEncoding = "br, gzip, deflate";

        await middleware.InvokeAsync(context);

        Assert.Equal(new StringValues("gzip"), context.Response.Headers.ContentEncoding);
    }
}

