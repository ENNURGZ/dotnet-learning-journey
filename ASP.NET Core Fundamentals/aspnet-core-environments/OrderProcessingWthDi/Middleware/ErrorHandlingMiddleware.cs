using System.Net;
using System.Text.Json;
using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Models.Exceptions;

namespace OrderProcessingWithDi.Middleware;

/// <summary>
/// Middleware for global error handling.
/// Catches exceptions and converts them to standardized error responses.
/// </summary>
public class ErrorHandlingMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly RequestDelegate next;
    private readonly ILogger<ErrorHandlingMiddleware> logger;

    /// <summary>
    /// Initializes a new instance of the ErrorHandlingMiddleware class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger instance.</param>
    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    /// <summary>
    /// Invokes the middleware to handle errors.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
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
#pragma warning disable CA1031 // Do not catch general exception types - this is intentional for global error handling
        catch (Exception ex)
        {
            await this.HandleExceptionAsync(context, ex);
        }
#pragma warning restore CA1031
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var errorResponse = CreateErrorResponse(context, exception);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorResponse.Status;

        var json = JsonSerializer.Serialize(errorResponse, JsonOptions);
        await context.Response.WriteAsync(json);

        this.logger.LogError(
            exception,
            "Error occurred: {ErrorType} - {ErrorMessage}",
            exception.GetType().Name,
            exception.Message);
    }

    private static ErrorResponse CreateErrorResponse(HttpContext context, Exception exception)
    {
        return exception switch
        {
            OrderNotFoundException orderNotFound => new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "Order Not Found",
                Status = (int)HttpStatusCode.NotFound,
                Detail = exception.Message,
                Instance = context.Request.Path,
                Extensions = new Dictionary<string, object>
                {
                    { "orderId", orderNotFound.OrderId }
                }
            },
            InvalidOrderException => new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Invalid Order",
                Status = (int)HttpStatusCode.BadRequest,
                Detail = exception.Message,
                Instance = context.Request.Path
            },
            ArgumentException => new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Invalid Argument",
                Status = (int)HttpStatusCode.BadRequest,
                Detail = exception.Message,
                Instance = context.Request.Path
            },
            _ => new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "Internal Server Error",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = "An unexpected error occurred while processing your request.",
                Instance = context.Request.Path
            }
        };
    }
}

/// <summary>
/// Extension methods for registering error handling middleware.
/// </summary>
public static class ErrorHandlingMiddlewareExtensions
{
    /// <summary>
    /// Adds the ErrorHandlingMiddleware to the application pipeline.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <returns>The application builder for chaining.</returns>
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}

