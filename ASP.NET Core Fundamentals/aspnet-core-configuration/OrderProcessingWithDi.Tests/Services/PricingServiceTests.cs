using OrderProcessingWithDi.Models.Configuration;
using OrderProcessingWithDi.Services.Implementations;
using Microsoft.Extensions.Options;
using Xunit;

namespace OrderProcessingWithDi.Tests.Services;

/// <summary>
/// Unit tests for PricingService.
/// Tests discount calculation logic.
/// </summary>
public class PricingServiceTests
{
    private static PricingService CreateService(PricingOptions? options = null)
    {
        options ??= new PricingOptions
        {
            DiscountThreshold = 5,
            DiscountPercentage = 0.1m,
            MinimumOrderValue = 0m
        };
        
        var optionsWrapper = Options.Create(options);
        return new PricingService(optionsWrapper);
    }

    [Fact]
    public void CalculateTotal_QuantityBelowThreshold_NoDiscountApplied()
    {
        // Arrange
        var service = CreateService();
        
        // Act
        var result = service.CalculateTotal(10m, 4);
        
        // Assert
        Assert.Equal(40m, result);
    }
    
    [Fact]
    public void CalculateTotal_QuantityAboveThreshold_DiscountApplied()
    {
        // Arrange
        var service = CreateService();
        
        // Act
        var result = service.CalculateTotal(10m, 6);
        
        // Assert
        // 10 * 6 = 60, then 60 * 0.9 = 54
        Assert.Equal(54m, result);
    }
    
    [Fact]
    public void CalculateTotal_QuantityEqualsThreshold_NoDiscountApplied()
    {
        // Arrange
        var service = CreateService();
        
        // Act
        var result = service.CalculateTotal(10m, 5);
        
        // Assert
        Assert.Equal(50m, result);
    }
    
    [Fact]
    public void CalculateTotal_TotalBelowMinimumOrderValue_NoDiscountApplied()
    {
        // Arrange
        // Create service with minimum order value of 100
        var options = new PricingOptions
        {
            DiscountThreshold = 5,
            DiscountPercentage = 0.1m,
            MinimumOrderValue = 100m
        };
        var service = CreateService(options);
        
        // Act
        // Quantity 6 (above threshold) but total 60 is below minimum 100, so no discount
        var result = service.CalculateTotal(10m, 6);
        
        // Assert
        // 10 * 6 = 60, which is < 100, so no discount applied
        Assert.Equal(60m, result);
    }
    
    [Fact]
    public void CalculateTotal_TotalAboveMinimumOrderValue_DiscountApplied()
    {
        // Arrange
        // Create service with minimum order value of 50
        var options = new PricingOptions
        {
            DiscountThreshold = 5,
            DiscountPercentage = 0.1m,
            MinimumOrderValue = 50m
        };
        var service = CreateService(options);
        
        // Act
        // Quantity 6 (above threshold) and total 60 is >= 50, so discount applies
        var result = service.CalculateTotal(10m, 6);
        
        // Assert
        // Total is 60, which is >= 50, so discount applies: 60 * 0.9 = 54
        Assert.Equal(54m, result);
    }
    
    [Fact]
    public void CalculateTotal_WithCustomOptions_UsesConfigurationValues()
    {
        // Arrange
        // Test that service uses configuration values correctly
        var options = new PricingOptions
        {
            DiscountThreshold = 10,
            DiscountPercentage = 0.2m,
            MinimumOrderValue = 50m
        };
        var service = CreateService(options);
        
        // Act
        // Quantity 11 (above threshold 10) and total 110 is >= 50, so discount applies
        var result = service.CalculateTotal(10m, 11);
        
        // Assert
        // 10 * 11 = 110, then 110 * (1 - 0.2) = 110 * 0.8 = 88
        Assert.Equal(88m, result);
    }
    
    [Fact]
    public void CalculateTotal_ZeroPrice_ReturnsZero()
    {
        // Arrange
        var service = CreateService();
        
        // Act
        var result = service.CalculateTotal(0m, 10);
        
        // Assert
        Assert.Equal(0m, result);
    }
    
    [Fact]
    public void CalculateTotal_ZeroQuantity_ReturnsZero()
    {
        // Arrange
        var service = CreateService();
        
        // Act
        var result = service.CalculateTotal(10m, 0);
        
        // Assert
        Assert.Equal(0m, result);
    }
}
