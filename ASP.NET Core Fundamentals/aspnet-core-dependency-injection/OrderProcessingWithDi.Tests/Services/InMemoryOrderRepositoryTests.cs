using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Services.Implementations;
using Xunit;

namespace OrderProcessingWithDi.Tests.Services;

/// <summary>
/// Unit tests for InMemoryOrderRepository.
/// Tests repository functionality and Singleton behavior.
/// </summary>
public class InMemoryOrderRepositoryTests
{
    private readonly InMemoryOrderRepository repository;
    
    public InMemoryOrderRepositoryTests()
    {
        this.repository = new InMemoryOrderRepository();
    }
    
    [Fact]
    public async Task SaveAsync_SavesOrder()
    {
        // Arrange
        var order = new OrderResult("PROD-001", 5, 50m);
        
        // Act
        await this.repository.SaveAsync(order);
        
        // Assert
        var allOrders = this.repository.GetAll();
        Assert.Single(allOrders);
        Assert.Equal(order, allOrders[0]);
    }
    
    [Fact]
    public async Task SaveAsync_MultipleOrders_SavesAll()
    {
        // Arrange
        var order1 = new OrderResult("PROD-001", 5, 50m);
        var order2 = new OrderResult("PROD-002", 3, 30m);
        var order3 = new OrderResult("PROD-003", 10, 100m);
        
        // Act
        await this.repository.SaveAsync(order1);
        await this.repository.SaveAsync(order2);
        await this.repository.SaveAsync(order3);
        
        // Assert
        var allOrders = this.repository.GetAll();
        Assert.Equal(3, allOrders.Count);
        Assert.Contains(order1, allOrders);
        Assert.Contains(order2, allOrders);
        Assert.Contains(order3, allOrders);
    }
    
    [Fact]
    public void GetAll_EmptyRepository_ReturnsEmptyList()
    {
        // Act
        var result = this.repository.GetAll();
        
        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task GetAll_ReturnsReadOnlyList()
    {
        // Arrange
        var order = new OrderResult("PROD-001", 5, 50m);
        await this.repository.SaveAsync(order);
        
        // Act
        var result = this.repository.GetAll();
        
        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<OrderResult>>(result);
    }
    
}

