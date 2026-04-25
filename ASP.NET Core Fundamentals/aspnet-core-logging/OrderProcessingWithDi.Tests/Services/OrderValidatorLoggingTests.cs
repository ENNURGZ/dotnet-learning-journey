using Moq;
using OrderProcessingWithDi.Services.Implementations;
using Microsoft.Extensions.Logging;

namespace OrderProcessingWithDi.Tests.Services;

/// <summary>
/// Unit tests for OrderValidator logging functionality.
/// Tests that structured logging is properly implemented in OrderValidator.
///
/// NOTE: These tests require logging implementation from the Logging assignment.
/// In the template project, logging is not implemented yet (only TODO comments).
/// 
/// IMPORTANT: After implementing logging in OrderValidator:
/// 1. Remove the `Skip` attribute from all test methods (change `[Fact(Skip = "...")]` to `[Fact]`)
/// 2. Update the constructor call to include ILogger<OrderValidator> parameter
/// 3. Uncomment the Verify() calls below to check that logging methods are called with correct parameters
/// 4. Run `dotnet test` to verify all tests pass
/// </summary>
public class OrderValidatorLoggingTests
{
    private readonly Mock<ILogger<OrderValidator>> mockLogger;
    private readonly OrderValidator validator;

    public OrderValidatorLoggingTests()
    {
        this.mockLogger = new Mock<ILogger<OrderValidator>>();
        this.validator = new OrderValidator(this.mockLogger.Object);
    }

    [Fact]
    public void Validate_LogsDebug_WhenValidationStarts()
    {
        // Arrange
        var productId = "PROD-001";
        var quantity = 5;
        var unitPrice = 10m;

        // Act
        var result = this.validator.Validate(productId, quantity, unitPrice);

        // Assert
        Assert.True(result.IsValid);
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Validating order") &&
                                               v.ToString()!.Contains(productId) &&
                                               v.ToString()!.Contains(quantity.ToString()) &&
                                               v.ToString()!.Contains(unitPrice.ToString())),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void Validate_LogsDebug_WhenValidationPasses()
    {
        // Arrange
        var productId = "PROD-002";
        var quantity = 3;
        var unitPrice = 15m;

        // Act
        var result = this.validator.Validate(productId, quantity, unitPrice);

        // Assert
        Assert.True(result.IsValid);
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Validation passed for product") &&
                                               v.ToString()!.Contains(productId)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void Validate_LogsWarning_WhenProductIdIsEmpty()
    {
        // Arrange
        var productId = "";
        var quantity = 5;
        var unitPrice = 10m;
        var expectedMessage = "ProductId cannot be empty";

        // Act
        var result = this.validator.Validate(productId, quantity, unitPrice);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(expectedMessage, result.ErrorMessage);
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Validation failed") &&
                                               v.ToString()!.Contains(expectedMessage)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void Validate_LogsWarning_WhenQuantityIsZero()
    {
        // Arrange
        var productId = "PROD-003";
        var quantity = 0;
        var unitPrice = 10m;
        var expectedMessage = "Quantity must be greater than 0";

        // Act
        var result = this.validator.Validate(productId, quantity, unitPrice);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(expectedMessage, result.ErrorMessage);
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Validation failed") &&
                                               v.ToString()!.Contains(expectedMessage)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void Validate_LogsWarning_WhenQuantityIsNegative()
    {
        // Arrange
        var productId = "PROD-004";
        var quantity = -5;
        var unitPrice = 10m;
        var expectedMessage = "Quantity must be greater than 0";

        // Act
        var result = this.validator.Validate(productId, quantity, unitPrice);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(expectedMessage, result.ErrorMessage);
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Validation failed") &&
                                               v.ToString()!.Contains(expectedMessage)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void Validate_LogsWarning_WhenUnitPriceIsZero()
    {
        // Arrange
        var productId = "PROD-005";
        var quantity = 5;
        var unitPrice = 0m;
        var expectedMessage = "UnitPrice must be greater than 0";

        // Act
        var result = this.validator.Validate(productId, quantity, unitPrice);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(expectedMessage, result.ErrorMessage);
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Validation failed") &&
                                               v.ToString()!.Contains(expectedMessage)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void Validate_LogsWarning_WhenUnitPriceIsNegative()
    {
        // Arrange
        var productId = "PROD-006";
        var quantity = 5;
        var unitPrice = -10m;
        var expectedMessage = "UnitPrice must be greater than 0";

        // Act
        var result = this.validator.Validate(productId, quantity, unitPrice);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(expectedMessage, result.ErrorMessage);
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Validation failed") &&
                                               v.ToString()!.Contains(expectedMessage)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
