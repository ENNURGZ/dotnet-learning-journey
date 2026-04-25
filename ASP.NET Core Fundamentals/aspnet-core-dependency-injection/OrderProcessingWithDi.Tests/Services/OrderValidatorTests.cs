using OrderProcessingWithDi.Services.Implementations;
using Xunit;

namespace OrderProcessingWithDi.Tests.Services;

/// <summary>
/// Unit tests for OrderValidator.
/// Tests all validation scenarios.
/// </summary>
public class OrderValidatorTests
{
    private readonly OrderValidator validator = new();

    [Fact]
    public void Validate_ValidOrder_ReturnsValid()
    {
        // Act
        var result = validator.Validate("PROD-001", 5, 10.50m);
        
        // Assert
        Assert.True(result.IsValid);
        Assert.Null(result.ErrorMessage);
    }
    
    [Fact]
    public void Validate_EmptyProductId_ReturnsInvalid()
    {
        // Act
        var result = validator.Validate("", 5, 10.50m);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("ProductId cannot be empty", result.ErrorMessage);
    }
    
    [Fact]
    public void Validate_NullProductId_ReturnsInvalid()
    {
        // Act
        var result = validator.Validate(null!, 5, 10.50m);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("ProductId cannot be empty", result.ErrorMessage);
    }
    
    [Fact]
    public void Validate_WhitespaceProductId_ReturnsInvalid()
    {
        // Act
        var result = validator.Validate("   ", 5, 10.50m);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("ProductId cannot be empty", result.ErrorMessage);
    }
    
    [Fact]
    public void Validate_ZeroQuantity_ReturnsInvalid()
    {
        // Act
        var result = validator.Validate("PROD-001", 0, 10.50m);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Quantity must be greater than 0", result.ErrorMessage);
    }
    
    [Fact]
    public void Validate_NegativeQuantity_ReturnsInvalid()
    {
        // Act
        var result = validator.Validate("PROD-001", -5, 10.50m);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Quantity must be greater than 0", result.ErrorMessage);
    }
    
    [Fact]
    public void Validate_ZeroUnitPrice_ReturnsInvalid()
    {
        // Act
        var result = validator.Validate("PROD-001", 5, 0m);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("UnitPrice must be greater than 0", result.ErrorMessage);
    }
    
    [Fact]
    public void Validate_NegativeUnitPrice_ReturnsInvalid()
    {
        // Act
        var result = validator.Validate("PROD-001", 5, -10.50m);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("UnitPrice must be greater than 0", result.ErrorMessage);
    }
    
    [Fact]
    public void Validate_MultipleInvalidFields_ReturnsFirstError()
    {
        // Act
        var result = validator.Validate("", -5, -10.50m);
        
        // Assert
        Assert.False(result.IsValid);
        // Should return first validation error (ProductId)
        Assert.Equal("ProductId cannot be empty", result.ErrorMessage);
    }
    
}

