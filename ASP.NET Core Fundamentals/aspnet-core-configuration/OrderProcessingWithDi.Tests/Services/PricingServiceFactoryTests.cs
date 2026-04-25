using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrderProcessingWithDi.Models.Configuration;
using OrderProcessingWithDi.Services.Implementations;
using OrderProcessingWithDi.Services.Interfaces;
using Xunit;

namespace OrderProcessingWithDi.Tests.Services;

/// <summary>
/// Unit tests for PricingServiceFactory.
/// Tests Factory Pattern implementation.
/// </summary>
public class PricingServiceFactoryTests
{
    private static void RegisterPricingOptions(IServiceCollection services)
    {
        var options = new PricingOptions
        {
            DiscountThreshold = 5,
            DiscountPercentage = 0.1m,
            MinimumOrderValue = 0m
        };
        services.Configure<PricingOptions>(opts =>
        {
            opts.DiscountThreshold = options.DiscountThreshold;
            opts.DiscountPercentage = options.DiscountPercentage;
            opts.MinimumOrderValue = options.MinimumOrderValue;
        });
    }

    [Fact]
    public void CreatePricingService_StandardType_ReturnsRegisteredService()
    {
        // Arrange
        var services = new ServiceCollection();
        RegisterPricingOptions(services);
        services.AddTransient<IPricingService, PricingService>();
        services.AddSingleton<IPricingServiceFactory, PricingServiceFactory>();
        
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IPricingServiceFactory>();
        
        // Act
        var service = factory.CreatePricingService("standard");
        
        // Assert
        Assert.NotNull(service);
        Assert.IsType<PricingService>(service);
    }
    
    [Fact]
    public void CreatePricingService_SimpleType_ReturnsSimpleService()
    {
        // Arrange
        var services = new ServiceCollection();
        RegisterPricingOptions(services);
        services.AddTransient<IPricingService, PricingService>();
        services.AddSingleton<IPricingServiceFactory, PricingServiceFactory>();
        
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IPricingServiceFactory>();
        
        // Act
        var service = factory.CreatePricingService("simple");
        
        // Assert
        Assert.NotNull(service);
        Assert.IsType<SimplePricingService>(service);
    }
    
    [Fact]
    public void CreatePricingService_NullType_UsesDefaultStandard()
    {
        // Arrange
        var services = new ServiceCollection();
        RegisterPricingOptions(services);
        services.AddTransient<IPricingService, PricingService>();
        services.AddSingleton<IPricingServiceFactory, PricingServiceFactory>();
        
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IPricingServiceFactory>();
        
        // Act
        var service = factory.CreatePricingService(null);
        
        // Assert
        Assert.NotNull(service);
        Assert.IsType<PricingService>(service);
    }
    
    [Fact]
    public void CreatePricingService_UnknownType_ThrowsArgumentException()
    {
        // Arrange
        var services = new ServiceCollection();
        RegisterPricingOptions(services);
        services.AddTransient<IPricingService, PricingService>();
        services.AddSingleton<IPricingServiceFactory, PricingServiceFactory>();
        
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IPricingServiceFactory>();
        
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => factory.CreatePricingService("unknown"));
        
        Assert.Contains("Unknown pricing service type", exception.Message);
    }
}
