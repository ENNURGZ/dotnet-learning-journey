using OrderProcessingWithDi.Middleware;
using Microsoft.Extensions.Options;
using OrderProcessingWithDi.Models.Configuration;
using OrderProcessingWithDi.Models.Exceptions;
using OrderProcessingWithDi.Services.Implementations;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi;

public class Program
{
    protected Program()
    {
    }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuration Options Pattern registration
        builder.Services.Configure<PricingOptions>(builder.Configuration.GetSection("Pricing"));
        builder.Services.Configure<OrderProcessingOptions>(builder.Configuration.GetSection("OrderProcessing"));
        builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection("Application"));

        // Service registrations
        builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        builder.Services.AddScoped<IOrderValidator, OrderValidator>();
        builder.Services.AddTransient<IPricingService, PricingService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddSingleton<IPricingServiceFactory, PricingServiceFactory>();

        // Lifetime Demo registrations
        builder.Services.AddSingleton<ILifetimeDemoService, LifetimeDemoService>();
        builder.Services.AddScoped<ILifetimeDemoService, LifetimeDemoService>();
        builder.Services.AddTransient<ILifetimeDemoService, LifetimeDemoService>();

        var app = builder.Build();

        // Middleware registration
        app.UseErrorHandling();

        // Basic Endpoints
        app.MapPost("/orders", async (string productId, int quantity, decimal unitPrice, IOrderService orderService) =>
        {
            var result = await orderService.ProcessOrderAsync(productId, quantity, unitPrice);
            return Results.Ok(result);
        }).WithName("CreateOrder").WithTags("Orders");

        app.MapGet("/orders", (IOrderRepository repository) =>
        {
            return Results.Ok(repository.GetAll());
        }).WithName("GetOrders").WithTags("Orders");

        // API V1 Route Group
        var apiV1Group = app.MapGroup("/api/v1/orders").WithTags("Orders V1");

        apiV1Group.MapGet("/", (IOrderRepository repository) => Results.Ok(repository.GetAll()));

        apiV1Group.MapGet("/{id:int}", (int id, IOrderRepository repository) =>
        {
            var order = repository.GetById(id);
            if (order is null)
            {
                throw new OrderNotFoundException(id);
            }
            return Results.Ok(order);
        });

        apiV1Group.MapGet("/product/{productId}", (string productId, IOrderRepository repository) =>
        {
            var orders = repository.GetAll().Where(o => o.ProductId == productId).ToList();
            return Results.Ok(orders);
        });

        apiV1Group.MapGet("/range/{minTotal:decimal}/{maxTotal:decimal}", (decimal minTotal, decimal maxTotal, IOrderRepository repository) =>
        {
            var orders = repository.GetAll().Where(o => o.Total >= minTotal && o.Total <= maxTotal).ToList();
            return Results.Ok(orders);
        });

        apiV1Group.MapGet("/search", (string? productId, decimal? minTotal, decimal? maxTotal, int? limit, IOrderRepository repository) =>
        {
            var query = repository.GetAll().AsEnumerable();
            if (!string.IsNullOrEmpty(productId)) query = query.Where(o => o.ProductId == productId);
            if (minTotal.HasValue) query = query.Where(o => o.Total >= minTotal.Value);
            if (maxTotal.HasValue) query = query.Where(o => o.Total <= maxTotal.Value);
            if (limit.HasValue) query = query.Take(limit.Value);
            return Results.Ok(query.ToList());
        });

        apiV1Group.MapGet("/recent/{days:int}", (int days, IOrderRepository repository) =>
        {
            if (days < 1 || days > 30) return Results.NotFound();
            return Results.Ok(repository.GetAll());
        });

        apiV1Group.MapGet("/stats", (IOrderRepository repository) =>
        {
            var orders = repository.GetAll();
            return Results.Ok(new
            {
                totalOrders = orders.Count,
                totalRevenue = orders.Sum(o => o.Total),
                averageOrderTotal = orders.Any() ? orders.Average(o => o.Total) : 0,
                mostOrderedProductId = orders.GroupBy(o => o.ProductId).OrderByDescending(g => g.Count()).FirstOrDefault()?.Key
            });
        });

        // Configuration Endpoints
        var configGroup = app.MapGroup("/config").WithTags("Configuration");

        configGroup.MapGet("/pricing", (IOptions<PricingOptions> options) => Results.Ok(options.Value));
        configGroup.MapGet("/order-processing", (IOptions<OrderProcessingOptions> options) => Results.Ok(options.Value));
        configGroup.MapGet("/application", (IOptions<ApplicationOptions> options) => Results.Ok(options.Value));
        configGroup.MapGet("/environment", (IWebHostEnvironment env, IConfiguration config) => Results.Ok(new
        {
            EnvironmentName = env.EnvironmentName,
            ApplicationName = env.ApplicationName,
            ContentRootPath = env.ContentRootPath,
            WebRootPath = env.WebRootPath,
            PricingFromEnv = config["Pricing:DiscountThreshold"]
        }));

        configGroup.MapGet("/all", (
            IOptions<PricingOptions> pricing,
            IOptions<OrderProcessingOptions> processing,
            IOptions<ApplicationOptions> appOptions) => Results.Ok(new
        {
            Pricing = pricing.Value,
            OrderProcessing = processing.Value,
            Application = appOptions.Value
        }));

        configGroup.MapGet("/raw", (IConfiguration config) => Results.Ok(new
        {
            PricingDiscountThreshold = config.GetValue<int>("Pricing:DiscountThreshold")
        }));

        // DI & Factory Demo Endpoints
        app.MapGet("/di-demo", (IEnumerable<ILifetimeDemoService> services, HttpResponse response) =>
        {
            var servicesList = services.ToList();
            
            // Registration order: 0: Singleton, 1: Scoped, 2: Transient
            var singleton = servicesList[0];
            var scoped = servicesList[1];
            var transient = servicesList[2];

            response.Headers.Append("X-DI-Singleton-Instance", singleton.InstanceId);

            return Results.Ok(new
            {
                singleton = new { singleton.InstanceId, singleton.CreatedAt, Lifetime = "Singleton" },
                scoped = new { scoped.InstanceId, scoped.CreatedAt, Lifetime = "Scoped" },
                transient = new { transient.InstanceId, transient.CreatedAt, Lifetime = "Transient" }
            });
        }).WithTags("DI Demo");

        app.MapGet("/factory-demo", (decimal price, int quantity, string? serviceType, IPricingServiceFactory factory) =>
        {
            var service = factory.CreatePricingService(serviceType);
            var total = service.CalculateTotal(price, quantity);
            return Results.Ok(new { total, serviceType = serviceType ?? "standard" });
        }).WithTags("Factory Demo");

        // Logging Demo Endpoints
        var loggingGroup = app.MapGroup("/logging").WithTags("Logging");

        loggingGroup.MapGet("/demo", (ILogger<Program> logger) =>
        {
            logger.LogTrace("This is a Trace message");
            logger.LogDebug("This is a Debug message");
            logger.LogInformation("This is an Information message");
            logger.LogWarning("This is a Warning message");
            logger.LogError("This is an Error message");
            logger.LogCritical("This is a Critical message");

            return Results.Ok(new { message = "Check application logs to see different log levels" });
        }).WithName("LoggingDemo");

        loggingGroup.MapGet("/structured", (ILogger<Program> logger, string? userId, string? action) =>
        {
            userId ??= "user123";
            action ??= "GetOrders";
            var timestamp = DateTime.UtcNow;

            logger.LogInformation("Structured log example: UserId={UserId}, Action={Action}, Timestamp={Timestamp}",
                userId, action, timestamp);

            return Results.Ok(new { userId, action, timestamp });
        }).WithName("StructuredLoggingDemo");

        loggingGroup.MapGet("/scopes", (ILogger<Program> logger) =>
        {
            using (logger.BeginScope("RequestId: {RequestId}", Guid.NewGuid()))
            {
                logger.LogInformation("Processing request");

                using (logger.BeginScope("Operation: {Operation}", "GetOrders"))
                {
                    logger.LogInformation("Executing operation");
                }

                logger.LogInformation("Request completed");
            }

            return Results.Ok(new { message = "Check logs for scoped logging demonstration" });
        }).WithName("LoggingScopesDemo");

        app.Run();
    }
}
