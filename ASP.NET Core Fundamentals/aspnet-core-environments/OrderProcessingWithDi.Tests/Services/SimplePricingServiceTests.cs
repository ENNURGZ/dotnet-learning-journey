using OrderProcessingWithDi.Services;
using OrderProcessingWithDi.Services.Implementations;
using Xunit;

namespace OrderProcessingWithDi.Tests.Services;

/// <summary>
/// Unit tests for SimplePricingService.
/// Tests basic pricing calculation without discounts.
/// </summary>
public class SimplePricingServiceTests
{
    [Fact]
    public void CalculateTotal_MultipliesPriceByQuantity()
    {
        // Arrange
        var service = new SimplePricingService();
        
        // Act
        var result = service.CalculateTotal(10m, 5);
        
        // Assert
        Assert.Equal(50m, result);
    }
    
    [Fact]
    public void CalculateTotal_LargeQuantity_NoDiscountApplied()
    {
        // Arrange
        var service = new SimplePricingService();
        
        // Act
        var result = service.CalculateTotal(10m, 100);
        
        // Assert
        // Simple service doesn't apply discounts, even for large quantities
        Assert.Equal(1000m, result);
    }
    
    [Fact]
    public void CalculateTotal_DecimalPrice_CalculatesCorrectly()
    {
        // Arrange
        var service = new SimplePricingService();
        
        // Act
        var result = service.CalculateTotal(9.99m, 3);
        
        // Assert
        Assert.Equal(29.97m, result);
    }
    
    [Fact]
    public void CalculateTotal_ZeroPrice_ReturnsZero()
    {
        // Arrange
        var service = new SimplePricingService();
        
        // Act
        var result = service.CalculateTotal(0m, 10);
        
        // Assert
        Assert.Equal(0m, result);
    }
    
    [Fact]
    public void CalculateTotal_ZeroQuantity_ReturnsZero()
    {
        // Arrange
        var service = new SimplePricingService();
        
        // Act
        var result = service.CalculateTotal(10m, 0);
        
        // Assert
        Assert.Equal(0m, result);
    }
}
