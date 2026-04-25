using Microsoft.Extensions.DependencyInjection;
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
    [Fact]
    public void CreatePricingService_StandardType_ReturnsRegisteredService()
    {
        // Arrange
        var services = new ServiceCollection();
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

