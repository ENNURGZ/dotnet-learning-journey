using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Middleware;

/// <summary>
/// Middleware demonstrating DI in ASP.NET Core middleware.
/// Shows how services are injected into middleware via constructor.
/// </summary>
public class DependencyInjectionDemoMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILifetimeDemoService singletonService;
    private readonly ILifetimeDemoService scopedService;
    private readonly ILifetimeDemoService transientService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyInjectionDemoMiddleware"/> class.
    /// Constructor injection in middleware - services are resolved per request
    /// </summary>
    public DependencyInjectionDemoMiddleware(
        RequestDelegate next,
        ILifetimeDemoService singletonService,
        ILifetimeDemoService scopedService,
        ILifetimeDemoService transientService)
    {
        this.next = next;
        this.singletonService = singletonService;
        this.scopedService = scopedService;
        this.transientService = transientService;
    }

    /// <summary>
    /// Invokes the middleware to demonstrate DI lifetimes.
    /// Adds instance IDs to response headers for each lifetime type.
    /// </summary>
    /// <param name="context">The HTTP context for the request.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        ValidateContext(context);
        await this.ProcessRequestAsync(context);
    }

    private static void ValidateContext(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }

    private async Task ProcessRequestAsync(HttpContext context)
    {
        // Add instance IDs to response headers for demonstration
        context.Response.Headers.Append("X-DI-Singleton-Instance", this.singletonService.InstanceId);
        context.Response.Headers.Append("X-DI-Scoped-Instance", this.scopedService.InstanceId);
        context.Response.Headers.Append("X-DI-Transient-Instance", this.transientService.InstanceId);

        await this.next(context);
    }
}

/// <summary>
/// Extension method for registering middleware.
/// </summary>
public static class DependencyInjectionDemoMiddlewareExtensions
{
    /// <summary>
    /// Adds the DependencyInjectionDemoMiddleware to the application pipeline.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <returns>The application builder for chaining.</returns>
    public static IApplicationBuilder UseDependencyInjectionDemo(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DependencyInjectionDemoMiddleware>();
    }
}
