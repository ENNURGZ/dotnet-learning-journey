using OrderProcessingWithDi.Middleware;
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

        // Register services with correct lifetimes:
        builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        builder.Services.AddSingleton<ISingletonService, LifetimeDemoService>();
        builder.Services.AddScoped<IScopedService, LifetimeDemoService>();
        builder.Services.AddTransient<ITransientService, LifetimeDemoService>();

        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddTransient<IPricingService, PricingService>();
        builder.Services.AddScoped<IOrderValidator, OrderValidator>();
        builder.Services.AddSingleton<IPricingServiceFactory, PricingServiceFactory>();

        var app = builder.Build();

        // ASPECT 4: Middleware with DI
        app.UseDependencyInjectionDemo();

        // ASPECT 5: Minimal API with Method Injection
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

        app.MapGet("/orders/{orderId:int}", (int orderId, IOrderRepository repository) =>
        {
            var order = repository.GetById(orderId);
            return order is not null ? Results.Ok(order) : Results.NotFound();
        }).WithName("GetOrderByIdLegacy").WithTags("Orders");

        app.MapGet("/orders/product/{productId:minlength(1)}", (string productId, IOrderRepository repository) =>
        {
            var orders = repository.GetAll().Where(o => o.ProductId == productId).ToList();
            return Results.Ok(orders);
        }).WithName("GetOrdersByProductIdLegacy").WithTags("Orders");

        app.MapGet("/orders/range/{minTotal:decimal}/{maxTotal:decimal}", (decimal minTotal, decimal maxTotal, IOrderRepository repository) =>
        {
            var orders = repository.GetAll().Where(o => o.Total >= minTotal && o.Total <= maxTotal).ToList();
            return Results.Ok(orders);
        }).WithName("GetOrdersByTotalRangeLegacy").WithTags("Orders");

        app.MapGet("/di-demo", (
            ISingletonService singleton,
            IScopedService scoped,
            ITransientService transient) =>
        {
            return Results.Ok(new
            {
                Singleton = new { singleton.InstanceId, singleton.CreatedAt },
                Scoped = new { scoped.InstanceId, scoped.CreatedAt },
                Transient = new { transient.InstanceId, transient.CreatedAt }
            });
        });

        app.MapGet("/factory-demo", (decimal price, int quantity, string? serviceType, IPricingServiceFactory factory) =>
        {
            var pricingService = factory.CreatePricingService(serviceType);
            var total = pricingService.CalculateTotal(price, quantity);
            return Results.Ok(new
            {
                message = "Pricing calculated via factory",
                serviceType = serviceType ?? "standard (default)",
                price,
                quantity,
                total,
                explanation = serviceType == "simple" ? "No discount applied" : "Standard discount applied if quantity > 5"
            });
        });

        // ROUTING TASK - Route Groups
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
            return order is not null ? Results.Ok(order) : Results.NotFound();
        }).WithName("GetOrderById");

        ordersGroup.MapGet("/product/{productId:minlength(1)}", (string productId, IOrderRepository repository) =>
        {
            var orders = repository.GetAll().Where(o => o.ProductId == productId).ToList();
            return Results.Ok(orders);
        }).WithName("GetOrdersByProductId");

        ordersGroup.MapGet("/range/{minTotal:decimal}/{maxTotal:decimal}", (decimal minTotal, decimal maxTotal, IOrderRepository repository) =>
        {
            var orders = repository.GetAll().Where(o => o.Total >= minTotal && o.Total <= maxTotal).ToList();
            return Results.Ok(orders);
        }).WithName("GetOrdersByTotalRange");

        ordersGroup.MapGet("/search", (string? productId, decimal? minTotal, decimal? maxTotal, int? limit, IOrderRepository repository) =>
        {
            var orders = repository.GetAll().AsEnumerable();
            if (!string.IsNullOrEmpty(productId))
            {
                orders = orders.Where(o => o.ProductId == productId);
            }

            if (minTotal.HasValue)
            {
                orders = orders.Where(o => o.Total >= minTotal.Value);
            }

            if (maxTotal.HasValue)
            {
                orders = orders.Where(o => o.Total <= maxTotal.Value);
            }

            if (limit.HasValue)
            {
                orders = orders.Take(limit.Value);
            }

            return Results.Ok(orders.ToList());
        }).WithName("SearchOrders");

        ordersGroup.MapGet("/recent/{days:int:range(1,30)}", (int days, IOrderRepository repository) =>
        {
            var orders = repository.GetAll();
            return Results.Ok(orders);
        }).WithName("GetRecentOrders");

        ordersGroup.MapGet("/stats", (HttpContext context, IOrderRepository repository) =>
        {
            var orders = repository.GetAll();
            var totalOrders = orders.Count;
            var totalRevenue = orders.Sum(o => o.Total);
            var averageOrderTotal = totalOrders > 0 ? totalRevenue / totalOrders : 0;
            var mostOrderedProductId = orders
                .GroupBy(o => o.ProductId)
                .OrderByDescending(g => g.Sum(o => o.Quantity))
                .Select(g => g.Key)
                .FirstOrDefault() ?? "None";

            return Results.Ok(new
            {
                totalOrders,
                totalRevenue,
                averageOrderTotal,
                mostOrderedProductId
            });
        }).WithName("GetOrderStatistics");

        ordersGroup.MapGet("/{id}", (string id, IOrderRepository repository) =>
        {
            if (int.TryParse(id, out int orderId))
            {
                var order = repository.GetById(orderId);
                if (order is not null)
                {
                    return Results.Ok(order);
                }
            }

            var orders = repository.GetAll().Where(o => o.ProductId == id).ToList();
            return orders.Any() ? Results.Ok(orders) : Results.NotFound();
        }).WithName("GetOrderByAnyId");

        app.Run();
    }
}
