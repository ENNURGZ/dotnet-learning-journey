using System;
using MiddlewareLibrary.Middleware;
using Microsoft.AspNetCore.Builder;

namespace MiddlewareLibrary.Extensions;

/// <summary>
/// Extension methods that register custom middleware components in the application pipeline.
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Adds the <see cref="RequestLoggingMiddleware"/> to the request pipeline.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <returns>The updated application builder instance.</returns>
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }

    /// <summary>
    /// Adds the <see cref="ErrorHandlingMiddleware"/> to the request pipeline.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <returns>The updated application builder instance.</returns>
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }

    /// <summary>
    /// Adds the <see cref="ConfigurableMiddleware"/> to the request pipeline using the provided configuration.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <param name="configureOptions">An action that configures middleware options.</param>
    /// <returns>The updated application builder instance.</returns>
    public static IApplicationBuilder UseConfigurableMiddleware(
        this IApplicationBuilder builder,
        Action<ConfigurableMiddlewareOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var options = new ConfigurableMiddlewareOptions();
        configureOptions(options);
        return builder.UseMiddleware<ConfigurableMiddleware>(options);
    }

    /// <summary>
    /// Adds the <see cref="AuthenticationMiddleware"/> to the request pipeline for API key validation.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <returns>The updated application builder instance.</returns>
    public static IApplicationBuilder UseApiAuthentication(this IApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder.UseMiddleware<AuthenticationMiddleware>();
    }

    /// <summary>
    /// Adds the <see cref="CompressionMiddleware"/> to the request pipeline.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <returns>The updated application builder instance.</returns>
    public static IApplicationBuilder UseCustomCompression(this IApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder.UseMiddleware<CompressionMiddleware>();
    }

    /// <summary>
    /// Adds the <see cref="CachingMiddleware"/> to the request pipeline using the provided options.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <param name="configureOptions">An action that configures caching options.</param>
    /// <returns>The updated application builder instance.</returns>
    public static IApplicationBuilder UseCustomCaching(
        this IApplicationBuilder builder,
        Action<CachingOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var options = new CachingOptions();
        configureOptions(options);
        return builder.UseMiddleware<CachingMiddleware>(options);
    }

    /// <summary>
    /// Adds the <see cref="RateLimitingMiddleware"/> to the request pipeline using the provided configuration.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <param name="configureOptions">An action that configures rate limiting options.</param>
    /// <returns>The updated application builder instance.</returns>
    public static IApplicationBuilder UseCustomRateLimiting(
        this IApplicationBuilder builder,
        Action<RateLimitingOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var options = new RateLimitingOptions();
        configureOptions(options);
        return builder.UseMiddleware<RateLimitingMiddleware>(options);
    }
}

