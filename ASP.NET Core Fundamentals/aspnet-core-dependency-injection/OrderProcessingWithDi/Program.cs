using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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

        // SINGLETON
        builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        builder.Services.AddSingleton<ILifetimeDemoService>(sp => new LifetimeDemoService());

        // SCOPED
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IOrderValidator, OrderValidator>();
        builder.Services.AddScoped<ILifetimeDemoService>(sp => new LifetimeDemoService());

        // TRANSIENT
        builder.Services.AddTransient<IPricingService, PricingService>();
        builder.Services.AddTransient<ILifetimeDemoService>(sp => new LifetimeDemoService());

        // FACTORY (Singleton)
        builder.Services.AddSingleton<IPricingServiceFactory, PricingServiceFactory>();

        var app = builder.Build();

        // Middleware with DI
        app.UseDependencyInjectionDemo();

        // Minimal API with Method Injection

        app.MapPost("/orders", async (string productId, int quantity, decimal unitPrice, IOrderService orderService) =>
        {
            var result = await orderService.ProcessOrderAsync(productId, quantity, unitPrice);
            return Results.Ok(result);
        })
        .WithName("CreateOrder")
        .WithTags("Orders");

        app.MapGet("/orders", (IOrderRepository repository) =>
        {
            var orders = repository.GetAll();
            return Results.Ok(orders);
        })
        .WithName("GetAllOrders")
        .WithTags("Orders");

        app.MapGet("/di-demo", (ILifetimeDemoService singletonService, ILifetimeDemoService scopedService, ILifetimeDemoService transientService) =>
        {
            return Results.Ok(new
            {
                message = "Dependency Injection Lifetime Demonstration",
                singleton = new { instanceId = singletonService.InstanceId, createdAt = singletonService.CreatedAt },
                scoped = new { instanceId = scopedService.InstanceId, createdAt = scopedService.CreatedAt },
                transient = new { instanceId = transientService.InstanceId, createdAt = transientService.CreatedAt },
                explanation = new
                {
                    singleton = "Same instance across all requests.",
                    scoped = "Same instance within a request, but different across requests.",
                    transient = "New instance every time it is requested."
                }
            });
        })
        .WithName("DependencyInjectionDemo")
        .WithTags("DI Demo");

        app.MapGet("/factory-demo", (IPricingServiceFactory factory, decimal price, int quantity, string? serviceType) =>
        {
            var pricingService = factory.CreatePricingService(serviceType);
            var total = pricingService.CalculateTotal(price, quantity);
            return Results.Ok(new
            {
                message = "Factory Pattern Demonstration",
                serviceType = serviceType ?? "standard",
                price = price,
                quantity = quantity,
                total = total,
                explanation = "Factory creates a specific implementation based on the serviceType parameter."
            });
        })
        .WithName("FactoryDemo")
        .WithTags("DI Demo");

        app.Run();
    }
}
