using System.Text.Json;
using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Models.Exceptions;

namespace OrderProcessingWithDi.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ErrorHandlingMiddleware> logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Global error handler needs to catch all exceptions")]
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (OrderNotFoundException ex)
        {
            await this.HandleExceptionAsync(context, ex);
        }
        catch (InvalidOrderException ex)
        {
            await this.HandleExceptionAsync(context, ex);
        }
        catch (ArgumentException ex)
        {
            await this.HandleExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await this.HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var errorResponse = this.CreateErrorResponse(context, exception);

        this.logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorResponse.Status;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsJsonAsync(errorResponse, options);
    }

    private ErrorResponse CreateErrorResponse(HttpContext context, Exception exception)
    {
        var errorResponse = exception switch
        {
            OrderNotFoundException ex => new ErrorResponse
            {
                Status = 404,
                Type = "rfc7231#section-6.5.4",
                Title = "Order Not Found",
                Detail = ex.Message,
                Extensions = { ["orderId"] = ex.OrderId }
            },
            InvalidOrderException ex => new ErrorResponse
            {
                Status = 400,
                Type = "rfc7231#section-6.5.1",
                Title = "Invalid Order",
                Detail = ex.Message
            },
            ArgumentException ex => new ErrorResponse
            {
                Status = 400,
                Type = "rfc7231#section-6.5.1",
                Title = "Invalid Argument",
                Detail = ex.Message
            },
            _ => new ErrorResponse
            {
                Status = 500,
                Type = "rfc7231#section-6.6.1",
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred."
            }
        };

        errorResponse.Instance = context.Request.Path;
        return errorResponse;
    }
}

public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}



