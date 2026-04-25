using Moq;
using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Services.Implementations;
using OrderProcessingWithDi.Services.Interfaces;
using Xunit;

namespace OrderProcessingWithDi.Tests.Services;

/// <summary>
/// Unit tests for OrderService.
/// Tests service orchestration with mocked dependencies.
/// </summary>
public class OrderServiceTests
{
    private readonly Mock<IPricingService> mockPricingService;
    private readonly Mock<IOrderRepository> mockRepository;
    private readonly Mock<IOrderValidator> mockValidator;
    private readonly OrderService service;
    
    public OrderServiceTests()
    {
        this.mockPricingService = new Mock<IPricingService>();
        this.mockRepository = new Mock<IOrderRepository>();
        this.mockValidator = new Mock<IOrderValidator>();

        this.service = new OrderService(this.mockPricingService.Object, this.mockRepository.Object, this.mockValidator.Object);
    }
    
    [Fact]
    public async Task ProcessOrderAsync_ValidOrder_ProcessesSuccessfully()
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
        var result = await this.service.ProcessOrderAsync(productId, quantity, unitPrice);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.ProductId);
        Assert.Equal(quantity, result.Quantity);
        Assert.Equal(expectedTotal, result.Total);

        this.mockValidator.Verify(v => v.Validate(productId, quantity, unitPrice), Times.Once);
        this.mockPricingService.Verify(p => p.CalculateTotal(unitPrice, quantity), Times.Once);
        this.mockRepository.Verify(r => r.SaveAsync(It.Is<OrderResult>(o => 
            o.ProductId == productId && 
            o.Quantity == quantity && 
            o.Total == expectedTotal)), Times.Once);
    }
    
    [Fact]
    public async Task ProcessOrderAsync_InvalidOrder_ThrowsArgumentException()
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
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => this.service.ProcessOrderAsync(productId, quantity, unitPrice));
        
        Assert.Equal(errorMessage, exception.Message);

        this.mockValidator.Verify(v => v.Validate(productId, quantity, unitPrice), Times.Once);
        this.mockPricingService.Verify(p => p.CalculateTotal(It.IsAny<decimal>(), It.IsAny<int>()), Times.Never);
        this.mockRepository.Verify(r => r.SaveAsync(It.IsAny<OrderResult>()), Times.Never);
    }
    
}

