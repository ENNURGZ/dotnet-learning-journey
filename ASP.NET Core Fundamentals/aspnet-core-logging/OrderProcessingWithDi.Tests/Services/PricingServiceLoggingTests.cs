using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using OrderProcessingWithDi.Models.Configuration;
using OrderProcessingWithDi.Services.Implementations;

namespace OrderProcessingWithDi.Tests.Services;

/// <summary>
/// Unit tests for PricingService logging functionality.
/// Tests that structured logging is properly implemented in PricingService.
///
/// NOTE: These tests require logging implementation from the Logging assignment.
/// In the template project, logging is not implemented yet (only TODO comments).
/// 
/// IMPORTANT: After implementing logging in PricingService:
/// 1. Remove the `Skip` attribute from all test methods (change `[Fact(Skip = "...")]` to `[Fact]`)
/// 2. Update the constructor call to include ILogger<PricingService> parameter
/// 3. Uncomment the Verify() calls below to check that logging methods are called with correct parameters
/// 4. Run `dotnet test` to verify all tests pass
/// </summary>
public class PricingServiceLoggingTests
{
    private readonly Mock<ILogger<PricingService>> mockLogger;
    private readonly PricingService service;

    public PricingServiceLoggingTests()
    {
        this.mockLogger = new Mock<ILogger<PricingService>>();

        var options = Options.Create(new PricingOptions
        {
            DiscountThreshold = 5,
            DiscountPercentage = 0.1m,
            MinimumOrderValue = 0m
        });

        this.service = new PricingService(options, this.mockLogger.Object);
    }

    #region Logging on Calculation Start

    [Fact]
    public void CalculateTotal_LogsDebug_WhenCalculationStarts()
    {
        // Arrange
        var basePrice = 10m;
        var quantity = 3;

        // Act
        var result = this.service.CalculateTotal(basePrice, quantity);

        // Assert
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Calculating total for price") &&
                                               v.ToString()!.Contains(basePrice.ToString()) &&
                                               v.ToString()!.Contains(quantity.ToString())),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Logging when Discount is Applied

    [Fact]
    public void CalculateTotal_LogsInformation_WhenDiscountIsApplied()
    {
        // Arrange
        var basePrice = 10m;
        var quantity = 6; // Above threshold (5)
        var expectedFinalTotal = 54m; // 60 * 0.9

        // Act
        var result = this.service.CalculateTotal(basePrice, quantity);

        // Assert
        Assert.Equal(expectedFinalTotal, result);
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Discount applied") &&
                                               v.ToString()!.Contains("Original total") &&
                                               v.ToString()!.Contains("Discount") &&
                                               v.ToString()!.Contains("Final total")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void CalculateTotal_LogsInformation_WithCorrectDiscountValues()
    {
        // Arrange
        var basePrice = 20m;
        var quantity = 10; // Above threshold
        var expectedFinalTotal = 180m; // 200 * 0.9

        // Act
        var result = this.service.CalculateTotal(basePrice, quantity);

        // Assert
        Assert.Equal(expectedFinalTotal, result);
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Discount applied")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Logging when Discount is Not Applied

    [Fact]
    public void CalculateTotal_LogsDebug_WhenDiscountIsNotApplied()
    {
        // Arrange
        var basePrice = 10m;
        var quantity = 4; // Below threshold (5)
        var expectedTotal = 40m; // 10 * 4

        // Act
        var result = this.service.CalculateTotal(basePrice, quantity);

        // Assert
        Assert.Equal(expectedTotal, result);
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("No discount applied") &&
                                               v.ToString()!.Contains("Total")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void CalculateTotal_LogsDebug_WhenQuantityEqualsThreshold()
    {
        // Arrange
        var basePrice = 10m;
        var quantity = 5; // Exactly at threshold, but discount requires > threshold
        var expectedTotal = 50m; // 10 * 5

        // Act
        var result = this.service.CalculateTotal(basePrice, quantity);

        // Assert
        Assert.Equal(expectedTotal, result);
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("No discount applied")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion
}
