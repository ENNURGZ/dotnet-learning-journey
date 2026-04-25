using OrderProcessingWithDi.Middleware;
using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Models.Configuration;
using OrderProcessingWithDi.Models.Exceptions;
using OrderProcessingWithDi.Services.Implementations;
using OrderProcessingWithDi.Services.Interfaces;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Service registrations
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddTransient<IOrderValidator, OrderValidator>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Factory pattern
builder.Services.AddScoped<PricingService>();
builder.Services.AddScoped<IPricingServiceFactory, PricingServiceFactory>();
builder.Services.AddScoped<IPricingService>(sp => sp.GetRequiredService<IPricingServiceFactory>().CreatePricingService());

// Lifetime demo services
builder.Services.AddSingleton<ISingletonService, LifetimeDemoService>();
builder.Services.AddScoped<IScopedService, LifetimeDemoService>();
builder.Services.AddTransient<ITransientService, LifetimeDemoService>();

// Configuration Options Pattern
builder.Services.Configure<PricingOptions>(builder.Configuration.GetSection("Pricing"));
builder.Services.Configure<OrderProcessingOptions>(builder.Configuration.GetSection("OrderProcessing"));
builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection("Application"));

var app = builder.Build();

// Middleware
app.UseErrorHandling();
app.UseDependencyInjectionDemo();

// Basic Order Endpoints
app.MapPost("/orders", async (string productId, int quantity, decimal unitPrice, IOrderService orderService) =>
{
    var result = await orderService.ProcessOrderAsync(productId, quantity, unitPrice);
    return Results.Ok(result);
});

app.MapGet("/orders", (IOrderRepository repository) => Results.Ok(repository.GetAll()));

// DI and Factory Demo Endpoints
app.MapGet("/di-demo", (ISingletonService singleton, IScopedService scoped, ITransientService transient) =>
{
    return Results.Ok(new
    {
        SingletonInstanceId = singleton.InstanceId,
        ScopedInstanceId = scoped.InstanceId,
        TransientInstanceId = transient.InstanceId
    });
});

app.MapGet("/factory-demo", (decimal price, int quantity, string? serviceType, IPricingServiceFactory factory) =>
{
    var pricingService = factory.CreatePricingService(serviceType);
    var total = pricingService.CalculateTotal(price, quantity);
    return Results.Ok(new { ServiceType = serviceType ?? "standard", Total = total });
});

// Routing Endpoints (Route Group)
var ordersApi = app.MapGroup("/api/v1/orders");

ordersApi.MapGet("/", (IOrderRepository repository) => Results.Ok(repository.GetAll()));

ordersApi.MapGet("/{orderId:int}", (int orderId, IOrderRepository repository) =>
{
    var order = repository.GetById(orderId);
    return order != null ? Results.Ok(order) : throw new OrderNotFoundException(orderId);
});

ordersApi.MapGet("/product/{productId}", (string productId, IOrderRepository repository) =>
{
    var orders = repository.GetAll().Where(o => o.ProductId == productId);
    return Results.Ok(orders);
});

ordersApi.MapGet("/range/{minTotal:decimal}/{maxTotal:decimal}", (decimal minTotal, decimal maxTotal, IOrderRepository repository) =>
{
    var orders = repository.GetAll().Where(o => o.Total >= minTotal && o.Total <= maxTotal);
    return Results.Ok(orders);
});

ordersApi.MapGet("/search", (string? productId, decimal? minTotal, decimal? maxTotal, int? limit, IOrderRepository repository) =>
{
    var query = repository.GetAll().AsEnumerable();
    if (!string.IsNullOrEmpty(productId)) query = query.Where(o => o.ProductId == productId);
    if (minTotal.HasValue) query = query.Where(o => o.Total >= minTotal.Value);
    if (maxTotal.HasValue) query = query.Where(o => o.Total <= maxTotal.Value);
    if (limit.HasValue) query = query.Take(limit.Value);
    return Results.Ok(query);
});

ordersApi.MapGet("/recent/{days:int:range(1,30)}", (int days, IOrderRepository repository) =>
{
    var cutoff = DateTime.UtcNow.AddDays(-days);
    var orders = repository.GetAll().Where(o => o.ProcessedAt >= cutoff);
    return Results.Ok(orders);
});

ordersApi.MapGet("/stats", (IOrderRepository repository) =>
{
    var orders = repository.GetAll();
    var mostOrdered = orders.GroupBy(o => o.ProductId)
                           .OrderByDescending(g => g.Count())
                           .Select(g => g.Key)
                           .FirstOrDefault();

    return Results.Ok(new
    {
        TotalOrders = orders.Count,
        TotalRevenue = orders.Sum(o => o.Total),
        AverageOrderTotal = orders.Any() ? orders.Average(o => o.Total) : 0,
        MostOrderedProductId = mostOrdered
    });
});

// Configuration Endpoints
app.MapGet("/config/pricing", (IOptions<PricingOptions> options) => Results.Ok(options.Value));
app.MapGet("/config/order-processing", (IOptions<OrderProcessingOptions> options) => Results.Ok(options.Value));
app.MapGet("/config/application", (IOptions<ApplicationOptions> options) => Results.Ok(options.Value));
app.MapGet("/config/all", (IOptions<PricingOptions> pricing, IOptions<OrderProcessingOptions> processing, IOptions<ApplicationOptions> appOptions) =>
{
    return Results.Ok(new
    {
        Pricing = pricing.Value,
        OrderProcessing = processing.Value,
        Application = appOptions.Value
    });
});

app.MapGet("/config/raw", (IConfiguration config) =>
{
    return Results.Ok(new
    {
        DiscountThreshold = config.GetValue<int>("Pricing:DiscountThreshold"),
        MaxQuantity = config.GetValue<int>("OrderProcessing:MaxQuantity"),
        AppName = config.GetValue<string>("Application:ApplicationName")
    });
});

// ENVIRONMENTS - Environment Demo Endpoint
app.MapGet("/config/environment", (IConfiguration configuration, IWebHostEnvironment environment) =>
{
    return Results.Ok(new
    {
        EnvironmentName = environment.EnvironmentName,
        ApplicationName = environment.ApplicationName,
        ContentRootPath = environment.ContentRootPath,
        WebRootPath = environment.WebRootPath,
        PricingFromEnv = configuration["Pricing:DiscountThreshold"],
        AppSettingsValue = configuration["Application:ApplicationName"]
    });
})
.WithName("GetEnvironmentConfig")
.WithTags("Environment");

app.MapGet("/", () => "Hello");
app.Run();

namespace OrderProcessingWithDi
{
    public partial class Program { }
}
