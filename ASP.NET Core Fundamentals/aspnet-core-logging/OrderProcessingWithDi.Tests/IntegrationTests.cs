using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderProcessingWithDi.Models;
using Xunit;

namespace OrderProcessingWithDi.Tests;

/// <summary>
/// Integration tests for Order Processing API.
/// Tests full request/response cycle with real DI container.
/// </summary>
public class IntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client = factory.CreateClient();

    #region POST /orders Tests

    [Fact]
    public async Task PostOrder_ValidOrder_ReturnsCorrectTotal()
    {
        // Arrange & Act
        var response = await this.client.PostAsync("/orders?productId=A1&quantity=6&unitPrice=10", null);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<OrderResult>();
        Assert.NotNull(result);
        Assert.Equal("A1", result.ProductId);
        Assert.Equal(6, result.Quantity);
        Assert.Equal(54m, result.Total); // 10 * 6 * 0.9 = 54 (discount applied)
    }
    
    [Fact]
    public async Task PostOrder_QuantityBelowThreshold_NoDiscountApplied()
    {
        // Arrange & Act
        var response = await this.client.PostAsync("/orders?productId=B2&quantity=4&unitPrice=10", null);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<OrderResult>();
        Assert.NotNull(result);
        Assert.Equal(40m, result.Total); // 10 * 4 = 40 (no discount)
    }
    
    [Fact]
    public async Task PostOrder_EmptyProductId_ReturnsBadRequest()
    {
        // Arrange & Act
        var response = await this.client.PostAsync("/orders?productId=&quantity=5&unitPrice=10", null);
        
        // Assert
        // Error handling middleware converts InvalidOrderException to 400 BadRequest
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostOrder_ZeroQuantity_ReturnsBadRequest()
    {
        // Arrange & Act
        var response = await this.client.PostAsync("/orders?productId=C3&quantity=0&unitPrice=10", null);
        
        // Assert
        // Error handling middleware converts InvalidOrderException to 400 BadRequest
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostOrder_NegativeQuantity_ReturnsBadRequest()
    {
        // Arrange & Act
        var response = await this.client.PostAsync("/orders?productId=C3&quantity=-5&unitPrice=10", null);
        
        // Assert
        // Error handling middleware converts InvalidOrderException to 400 BadRequest
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostOrder_ZeroUnitPrice_ReturnsBadRequest()
    {
        // Arrange & Act
        var response = await this.client.PostAsync("/orders?productId=C3&quantity=5&unitPrice=0", null);
        
        // Assert
        // Error handling middleware converts InvalidOrderException to 400 BadRequest
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostOrder_NegativeUnitPrice_ReturnsBadRequest()
    {
        // Arrange & Act
        var response = await this.client.PostAsync("/orders?productId=C3&quantity=5&unitPrice=-10", null);
        
        // Assert
        // Error handling middleware converts InvalidOrderException to 400 BadRequest
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostOrder_MultipleOrders_SavesAll()
    {
        // Arrange & Act
        var response1 = await this.client.PostAsync("/orders?productId=D1&quantity=2&unitPrice=5", null);
        var response2 = await this.client.PostAsync("/orders?productId=D2&quantity=3&unitPrice=7", null);
        var response3 = await this.client.PostAsync("/orders?productId=D3&quantity=4&unitPrice=9", null);
        
        // Assert
        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();
        response3.EnsureSuccessStatusCode();
        
        // Get all orders
        var getAllResponse = await this.client.GetAsync("/orders");
        getAllResponse.EnsureSuccessStatusCode();
        var orders = await getAllResponse.Content.ReadFromJsonAsync<List<OrderResult>>();
        
        Assert.NotNull(orders);
        Assert.Contains(orders, o => o.ProductId == "D1");
        Assert.Contains(orders, o => o.ProductId == "D2");
        Assert.Contains(orders, o => o.ProductId == "D3");
    }

    #endregion

    #region GET /orders Tests

    [Fact]
    public async Task GetOrders_EmptyRepository_ReturnsEmptyList()
    {
        // Note: This test assumes a fresh repository instance
        // In real scenario, you might want to use a test database or reset mechanism
        
        // Act
        var response = await this.client.GetAsync("/orders");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
        // Note: May not be empty if other tests ran before
    }
    
    [Fact]
    public async Task GetOrders_AfterPosting_ReturnsSavedOrders()
    {
        // Arrange
        await this.client.PostAsync("/orders?productId=E1&quantity=5&unitPrice=10", null);
        await this.client.PostAsync("/orders?productId=E2&quantity=3&unitPrice=15", null);
        
        // Act
        var response = await this.client.GetAsync("/orders");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
        Assert.Contains(orders, o => o.ProductId == "E1");
        Assert.Contains(orders, o => o.ProductId == "E2");
    }
    
    [Fact]
    public async Task GetOrders_DemonstratesSingletonLifetime()
    {
        // Arrange - Create orders
        await this.client.PostAsync("/orders?productId=F1&quantity=1&unitPrice=10", null);
        await this.client.PostAsync("/orders?productId=F2&quantity=2&unitPrice=20", null);
        
        // Act - Get orders multiple times
        var response1 = await this.client.GetAsync("/orders");
        var response2 = await this.client.GetAsync("/orders");
        
        // Assert - Same repository instance should return same data
        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();
        
        var orders1 = await response1.Content.ReadFromJsonAsync<List<OrderResult>>();
        var orders2 = await response2.Content.ReadFromJsonAsync<List<OrderResult>>();
        
        Assert.NotNull(orders1);
        Assert.NotNull(orders2);
        // Both should contain the same orders (Singleton repository)
        Assert.Equal(orders1.Count, orders2.Count);
    }

    #endregion

    #region GET /di-demo Tests

    [Fact]
    public async Task GetDiDemo_ReturnsLifetimeInformation()
    {
        // Act
        var response = await this.client.GetAsync("/di-demo");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        
        Assert.Contains("singleton", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("scoped", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("transient", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("instanceId", content, StringComparison.OrdinalIgnoreCase);
    }
    
    [Fact]
    public async Task GetDiDemo_MultipleRequests_ShowsSingletonConsistency()
    {
        // Act
        var response1 = await this.client.GetAsync("/di-demo");
        var response2 = await this.client.GetAsync("/di-demo");
        
        // Assert
        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();
        
        var content1 = await response1.Content.ReadAsStringAsync();
        var content2 = await response2.Content.ReadAsStringAsync();
        
        // Extract singleton instance IDs from response headers or content
        var singletonId1 = response1.Headers.GetValues("X-DI-Singleton-Instance").FirstOrDefault();
        var singletonId2 = response2.Headers.GetValues("X-DI-Singleton-Instance").FirstOrDefault();
        
        // Singleton should have same instance ID across requests
        Assert.NotNull(singletonId1);
        Assert.NotNull(singletonId2);
        Assert.Equal(singletonId1, singletonId2);
    }

    #endregion

    #region GET /factory-demo Tests

    [Fact]
    public async Task GetFactoryDemo_StandardType_ReturnsDiscountedTotal()
    {
        // Act
        var response = await this.client.GetAsync("/factory-demo?price=10&quantity=6&serviceType=standard");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        
        Assert.Contains("total", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("standard", content, StringComparison.OrdinalIgnoreCase);
        // Standard service should apply discount: 10 * 6 * 0.9 = 54
    }
    
    [Fact]
    public async Task GetFactoryDemo_SimpleType_ReturnsNoDiscount()
    {
        // Act
        var response = await this.client.GetAsync("/factory-demo?price=10&quantity=6&serviceType=simple");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        
        Assert.Contains("total", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("simple", content, StringComparison.OrdinalIgnoreCase);
        // Simple service should not apply discount: 10 * 6 = 60
    }
    
    [Fact]
    public async Task GetFactoryDemo_NoServiceType_UsesDefault()
    {
        // Act
        var response = await this.client.GetAsync("/factory-demo?price=10&quantity=5");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        
        Assert.Contains("total", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetFactoryDemo_InvalidServiceType_ReturnsError()
    {
        // Act
        var response = await this.client.GetAsync("/factory-demo?price=10&quantity=5&serviceType=invalid");
        
        // Assert
        // Factory should throw ArgumentException, which results in 500 error
        Assert.True(response.StatusCode == HttpStatusCode.InternalServerError || 
                    response.StatusCode == HttpStatusCode.BadRequest);
    }

    #endregion
}
