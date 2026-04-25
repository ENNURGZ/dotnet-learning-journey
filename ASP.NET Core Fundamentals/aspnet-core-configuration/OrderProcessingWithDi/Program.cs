using OrderProcessingWithDi.Middleware;
using OrderProcessingWithDi.Models.Configuration;
using OrderProcessingWithDi.Models.Exceptions;
using OrderProcessingWithDi.Services.Implementations;
using OrderProcessingWithDi.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace OrderProcessingWithDi;

public class Program
{
    protected Program()
    {
    }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register configuration options
        builder.Services.Configure<PricingOptions>(builder.Configuration.GetSection(PricingOptions.SectionName));
        builder.Services.Configure<OrderProcessingOptions>(builder.Configuration.GetSection(OrderProcessingOptions.SectionName));
        builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection(ApplicationOptions.SectionName));

        // Register services
        builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        builder.Services.AddSingleton<IPricingServiceFactory, PricingServiceFactory>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IOrderValidator, OrderValidator>();
        builder.Services.AddTransient<IPricingService, PricingService>();

        // DI Demo services
        builder.Services.AddSingleton<ILifetimeDemoService, LifetimeDemoService>();
        builder.Services.AddScoped<ILifetimeDemoService, LifetimeDemoService>();
        builder.Services.AddTransient<ILifetimeDemoService, LifetimeDemoService>();

        var app = builder.Build();

        // Middleware
        app.UseErrorHandling();
        app.UseDependencyInjectionDemo();

        // Endpoints from previous assignments
        app.MapPost("/orders", async (string productId, int quantity, decimal unitPrice, IOrderService orderService) =>
        {
            var result = await orderService.ProcessOrderAsync(productId, quantity, unitPrice);
            return Results.Ok(result);
        }).WithName("CreateOrderLegacy").WithTags("Orders");

        app.MapGet("/orders", (IOrderRepository repository) =>
        {
            var orders = repository.GetAll();
            return Results.Ok(orders);
        }).WithName("GetAllOrdersLegacy").WithTags("Orders");

        app.MapGet("/di-demo", (ILifetimeDemoService singleton, ILifetimeDemoService scoped, ILifetimeDemoService transient) =>
        {
            return Results.Ok(new
            {
                SingletonInstanceId = singleton.InstanceId,
                ScopedInstanceId = scoped.InstanceId,
                TransientInstanceId = transient.InstanceId,
            });
        }).WithName("GetDiDemo").WithTags("DI Demo");

        app.MapGet("/factory-demo", (decimal price, int quantity, string? serviceType, IPricingServiceFactory factory) =>
        {
            var pricingService = factory.CreatePricingService(serviceType);
            var total = pricingService.CalculateTotal(price, quantity);
            return Results.Ok(new
            {
                Message = "Pricing calculated via factory",
                ServiceType = serviceType ?? "standard (default)",
                Price = price,
                Quantity = quantity,
                Total = total
            });
        }).WithName("GetFactoryDemo").WithTags("Factory Demo");

        // Routing Task - Route Groups
        var ordersGroup = app.MapGroup("/api/v1/orders").WithTags("Orders API v1");

        ordersGroup.MapPost("/", async (string productId, int quantity, decimal unitPrice, IOrderService orderService) =>
        {
            var result = await orderService.ProcessOrderAsync(productId, quantity, unitPrice);
            return Results.Ok(result);
        }).WithName("CreateOrder");

        ordersGroup.MapGet("/", (IOrderRepository repository) =>
        {
            var orders = repository.GetAll();
            return Results.Ok(orders);
        }).WithName("GetAllOrders");

        ordersGroup.MapGet("/{orderId:int}", (int orderId, IOrderRepository repository) =>
        {
            var order = repository.GetById(orderId);
            if (order == null)
            {
                throw new OrderNotFoundException(orderId);
            }

            return Results.Ok(order);
        }).WithName("GetOrderById");

        ordersGroup.MapGet("/product/{productId:minlength(1)}", (string productId, IOrderRepository repository) =>
        {
            var orders = repository.GetAll().Where(o => o.ProductId == productId);
            return Results.Ok(orders);
        }).WithName("GetOrdersByProductId");

        ordersGroup.MapGet("/range/{minTotal:decimal}/{maxTotal:decimal}", (decimal minTotal, decimal maxTotal, IOrderRepository repository) =>
        {
            var orders = repository.GetAll().Where(o => o.Total >= minTotal && o.Total <= maxTotal);
            return Results.Ok(orders);
        }).WithName("GetOrdersByTotalRange");

        ordersGroup.MapGet("/search", (string? productId, decimal? minTotal, decimal? maxTotal, int? limit, IOrderRepository repository) =>
        {
            var query = repository.GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(productId))
            {
                query = query.Where(o => o.ProductId == productId);
            }

            if (minTotal.HasValue)
            {
                query = query.Where(o => o.Total >= minTotal.Value);
            }

            if (maxTotal.HasValue)
            {
                query = query.Where(o => o.Total <= maxTotal.Value);
            }

            var orders = query.ToList();
            if (limit.HasValue)
            {
                orders = orders.Take(limit.Value).ToList();
            }

            return Results.Ok(orders);
        }).WithName("SearchOrders");

        ordersGroup.MapGet("/recent/{days:int:range(1,30)}", (int days, IOrderRepository repository) =>
        {
            var orders = repository.GetAll();
            return Results.Ok(orders);
        }).WithName("GetRecentOrders");

        ordersGroup.MapGet("/stats", (IOrderRepository repository) =>
        {
            var orders = repository.GetAll().ToList();
            if (!orders.Any())
            {
                return Results.Ok(new
                {
                    TotalOrders = 0,
                    TotalRevenue = 0m,
                    AverageOrderTotal = 0m,
                    MostOrderedProductId = string.Empty,
                });
            }

            var totalOrders = orders.Count;
            var totalRevenue = orders.Sum(o => o.Total);
            var averageOrderTotal = orders.Average(o => o.Total);
            var mostOrderedProductId = orders.GroupBy(o => o.ProductId)
                .OrderByDescending(g => g.Sum(o => o.Quantity))
                .First().Key;

            return Results.Ok(new
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                AverageOrderTotal = averageOrderTotal,
                MostOrderedProductId = mostOrderedProductId,
            });
        }).WithName("GetOrderStatistics");

        ordersGroup.MapGet("/{id}", (string id, IOrderRepository repository) =>
        {
            if (int.TryParse(id, out int orderId))
            {
                var order = repository.GetById(orderId);
                if (order != null)
                {
                    return Results.Ok(order);
                }
            }

            var productOrders = repository.GetAll().Where(o => o.ProductId == id);
            if (productOrders.Any())
            {
                return Results.Ok(productOrders);
            }

            return Results.NotFound();
        }).WithName("GetOrderByAnyId");

        // Configuration Demo Endpoints
        app.MapGet("/config/pricing", (IOptions<PricingOptions> options) =>
        {
            return Results.Ok(options.Value);
        }).WithName("GetPricingConfig").WithTags("Configuration");

        app.MapGet("/config/order-processing", (IOptions<OrderProcessingOptions> options) =>
        {
            return Results.Ok(options.Value);
        }).WithName("GetOrderProcessingConfig").WithTags("Configuration");

        app.MapGet("/config/application", (IOptions<ApplicationOptions> options) =>
        {
            return Results.Ok(options.Value);
        }).WithName("GetApplicationConfig").WithTags("Configuration");

        app.MapGet("/config/all", (IOptions<PricingOptions> pricing, IOptions<OrderProcessingOptions> orderProcessing, IOptions<ApplicationOptions> application) =>
        {
            return Results.Ok(new
            {
                pricing = pricing.Value,
                orderProcessing = orderProcessing.Value,
                application = application.Value
            });
        }).WithName("GetAllConfig").WithTags("Configuration");

        app.MapGet("/config/raw", (IConfiguration configuration) =>
        {
            return Results.Ok(new
            {
                pricing = configuration.GetSection(PricingOptions.SectionName).Get<PricingOptions>(),
                orderProcessing = configuration.GetSection(OrderProcessingOptions.SectionName).Get<OrderProcessingOptions>(),
                application = configuration.GetSection(ApplicationOptions.SectionName).Get<ApplicationOptions>()
            });
        }).WithName("GetRawConfig").WithTags("Configuration");

        app.Run();
    }
}
