using Microsoft.Extensions.Logging;
using Moq;
using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Models.Exceptions;
using OrderProcessingWithDi.Services.Implementations;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Tests.Services;

/// <summary>
/// Unit tests for OrderService logging functionality.
/// Tests that structured logging is properly implemented in OrderService.
///
/// NOTE: These tests require logging implementation from the Logging assignment.
/// In the template project, logging is not implemented yet (only TODO comments).
/// 
/// IMPORTANT: After implementing logging in OrderService:
/// 1. Remove the `Skip` attribute from all test methods (change `[Fact(Skip = "...")]` to `[Fact]`)
/// 2. Update the constructor call to include ILogger<OrderService> parameter
/// 3. Uncomment the Verify() calls below to check that logging methods are called with correct parameters
/// 4. Run `dotnet test` to verify all tests pass
/// </summary>
public class OrderServiceLoggingTests
{
    private readonly Mock<IPricingService> mockPricingService;
    private readonly Mock<IOrderRepository> mockRepository;
    private readonly Mock<IOrderValidator> mockValidator;
    private readonly Mock<ILogger<OrderService>> mockLogger;
    private readonly OrderService service;

    public OrderServiceLoggingTests()
    {
        this.mockPricingService = new Mock<IPricingService>();
        this.mockRepository = new Mock<IOrderRepository>();
        this.mockValidator = new Mock<IOrderValidator>();
        this.mockLogger = new Mock<ILogger<OrderService>>();

        // TODO: After implementing logging, update constructor to include ILogger<OrderService> parameter
        this.service = new OrderService(
            this.mockPricingService.Object,
            this.mockRepository.Object,
            this.mockValidator.Object,
            this.mockLogger.Object);
    }

    #region Logging on Order Processing Start

    [Fact]
    public async Task ProcessOrderAsync_LogsInformation_WhenProcessingStarts()
    {
        // Arrange
        var productId = "PROD-001";
        var quantity = 5;
        var unitPrice = 10m;
        var expectedTotal = 50m;

        this.mockValidator
            .Setup(v => v.Validate(productId, quantity, unitPrice))
            .Returns((true, (string?)null));

        this.mockPricingService
            .Setup(p => p.CalculateTotal(unitPrice, quantity))
            .Returns(expectedTotal);

        this.mockRepository
            .Setup(r => r.SaveAsync(It.IsAny<OrderResult>()))
            .Returns(Task.CompletedTask);

        // Act
        await this.service.ProcessOrderAsync(productId, quantity, unitPrice);

        // Assert
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Processing order for product") &&
                                               v.ToString()!.Contains(productId) &&
                                               v.ToString()!.Contains(quantity.ToString())),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Logging on Successful Order Processing

    [Fact]
    public async Task ProcessOrderAsync_LogsInformation_WhenOrderProcessedSuccessfully()
    {
        // Arrange
        var productId = "PROD-002";
        var quantity = 3;
        var unitPrice = 15m;
        var expectedTotal = 45m;

        this.mockValidator
            .Setup(v => v.Validate(productId, quantity, unitPrice))
            .Returns((true, (string?)null));

        this.mockPricingService
            .Setup(p => p.CalculateTotal(unitPrice, quantity))
            .Returns(expectedTotal);

        this.mockRepository
            .Setup(r => r.SaveAsync(It.IsAny<OrderResult>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await this.service.ProcessOrderAsync(productId, quantity, unitPrice);

        // Assert
        Assert.NotNull(result);
        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Order processed successfully") &&
                                               v.ToString()!.Contains(productId) &&
                                               v.ToString()!.Contains(expectedTotal.ToString())),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Logging on Validation Failure

    [Fact]
    public async Task ProcessOrderAsync_LogsWarning_WhenValidationFails()
    {
        // Arrange
        var productId = "";
        var quantity = 5;
        var unitPrice = 10m;
        var errorMessage = "ProductId cannot be empty";

        this.mockValidator
            .Setup(v => v.Validate(productId, quantity, unitPrice))
            .Returns((false, errorMessage));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOrderException>(
            () => this.service.ProcessOrderAsync(productId, quantity, unitPrice));

        Assert.Equal(errorMessage, exception.Message);

        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Order validation failed") &&
                                               v.ToString()!.Contains(errorMessage)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Logging on Exception

    [Fact]
    public async Task ProcessOrderAsync_LogsError_WhenExceptionOccurs()
    {
        // Arrange
        var productId = "PROD-003";
        var quantity = 5;
        var unitPrice = 10m;
        var exceptionMessage = "Repository error";

        this.mockValidator
            .Setup(v => v.Validate(productId, quantity, unitPrice))
            .Returns((true, (string?)null));

        this.mockPricingService
            .Setup(p => p.CalculateTotal(unitPrice, quantity))
            .Returns(50m);

        this.mockRepository
            .Setup(r => r.SaveAsync(It.IsAny<OrderResult>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => this.service.ProcessOrderAsync(productId, quantity, unitPrice));

        this.mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error processing order") &&
                                               v.ToString()!.Contains(exceptionMessage)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion
}
