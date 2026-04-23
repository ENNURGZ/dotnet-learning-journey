using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using MiddlewareLibrary.Middleware;

namespace MiddlewareLibrary.Tests;

public class ErrorHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ReturnsInternalServerError_OnException()
    {
        var middleware = new ErrorHandlingMiddleware(_ => throw new InvalidOperationException("Boom"), NullLogger<ErrorHandlingMiddleware>.Instance);
        var context = TestHelpers.CreateContext("/boom");

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        var body = await TestHelpers.ReadResponseBodyAsync(context.Response);
        Assert.Contains("Boom", body);
    }

    [Theory]
    [InlineData(typeof(ArgumentNullException), StatusCodes.Status400BadRequest)]
    [InlineData(typeof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized)]
    [InlineData(typeof(NotImplementedException), StatusCodes.Status501NotImplemented)]
    public async Task InvokeAsync_ReturnsExpectedStatus_ForKnownExceptions(Type exceptionType, int expectedStatus)
    {
        RequestDelegate next = _ => throw (Exception)Activator.CreateInstance(exceptionType, "failure")!;
        var middleware = new ErrorHandlingMiddleware(next, NullLogger<ErrorHandlingMiddleware>.Instance);
        var context = TestHelpers.CreateContext("/error");

        await middleware.InvokeAsync(context);

        Assert.Equal(expectedStatus, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);
        var body = await TestHelpers.ReadResponseBodyAsync(context.Response);
        Assert.Contains("failure", body);
    }
}

