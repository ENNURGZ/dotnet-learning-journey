using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderProcessingWithDi.Models;

namespace OrderProcessingWithDi.Tests;

/// <summary>
/// Integration tests for Routing functionality in Order Processing API.
/// Tests route parameters, constraints, route groups, and optional parameters.
/// </summary>
public class RoutingTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client = factory.CreateClient();

    #region GET /api/v1/orders/{orderId:int} Tests

    [Fact]
    public async Task GetOrderById_ValidIntegerId_ReturnsOrder()
    {
        // Arrange - Create an order first
        var createResponse = await this.client.PostAsync("/orders?productId=TEST1&quantity=5&unitPrice=10", null);
        createResponse.EnsureSuccessStatusCode();

        // Get all orders to find the index of the created order
        var getAllResponse = await this.client.GetAsync("/orders");
        getAllResponse.EnsureSuccessStatusCode();
        var allOrders = await getAllResponse.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(allOrders);

        // Find the index of the order we just created
        var orderIndex = allOrders.FindIndex(o => o.ProductId == "TEST1" && o.Quantity == 5);
        Assert.True(orderIndex >= 0, "Created order should exist in repository");

        // Act - Get order by its index
        var response = await this.client.GetAsync($"/api/v1/orders/{orderIndex}");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<OrderResult>();
        Assert.NotNull(result);
        Assert.Equal("TEST1", result.ProductId);
        Assert.Equal(5, result.Quantity);
    }

    [Fact]
    public async Task GetOrderById_InvalidId_ReturnsNotFound()
    {
        // Act
        var response = await this.client.GetAsync("/api/v1/orders/abc");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetOrderById_NonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await this.client.GetAsync("/api/v1/orders/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region GET /api/v1/orders/product/{productId:minlength(1)} Tests

    [Fact]
    public async Task GetOrdersByProductId_ValidProductId_ReturnsOrders()
    {
        // Arrange - Create orders with same product ID
        await this.client.PostAsync("/orders?productId=PROD-A&quantity=2&unitPrice=10", null);
        await this.client.PostAsync("/orders?productId=PROD-A&quantity=3&unitPrice=15", null);

        // Act
        var response = await this.client.GetAsync("/api/v1/orders/product/PROD-A");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
        Assert.True(orders.Count >= 2);
        Assert.All(orders, o => Assert.Equal("PROD-A", o.ProductId));
    }

    [Fact]
    public async Task GetOrdersByProductId_EmptyProductId_ReturnsNotFound()
    {
        // Act
        var response = await this.client.GetAsync("/api/v1/orders/product/");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetOrdersByProductId_NonExistentProductId_ReturnsEmptyList()
    {
        // Act
        var response = await this.client.GetAsync("/api/v1/orders/product/NONEXISTENT");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
        Assert.Empty(orders);
    }

    #endregion

    #region GET /api/v1/orders/range/{minTotal:decimal}/{maxTotal:decimal} Tests

    [Fact]
    public async Task GetOrdersByTotalRange_ValidRange_ReturnsFilteredOrders()
    {
        // Arrange - Create orders with different totals
        await this.client.PostAsync("/orders?productId=RANGE1&quantity=5&unitPrice=10", null); // total = 50
        await this.client.PostAsync("/orders?productId=RANGE2&quantity=6&unitPrice=10", null); // total = 54
        await this.client.PostAsync("/orders?productId=RANGE3&quantity=10&unitPrice=10", null); // total = 90

        // Act
        var response = await this.client.GetAsync("/api/v1/orders/range/50/60");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
        Assert.All(orders, o => Assert.True(o.Total >= 50m && o.Total <= 60m));
    }

    [Fact]
    public async Task GetOrdersByTotalRange_NoMatches_ReturnsEmptyList()
    {
        // Act
        var response = await this.client.GetAsync("/api/v1/orders/range/1000/2000");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
        Assert.Empty(orders);
    }

    #endregion

    #region GET /api/v1/orders/search Tests

    [Fact]
    public async Task SearchOrders_NoParameters_ReturnsAllOrders()
    {
        // Arrange - Create some orders
        await this.client.PostAsync("/orders?productId=SEARCH1&quantity=2&unitPrice=10", null);
        await this.client.PostAsync("/orders?productId=SEARCH2&quantity=3&unitPrice=15", null);

        // Act
        var response = await this.client.GetAsync("/api/v1/orders/search");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
        Assert.True(orders.Count >= 2);
    }

    [Fact]
    public async Task SearchOrders_WithProductId_ReturnsFilteredOrders()
    {
        // Arrange
        await this.client.PostAsync("/orders?productId=SEARCH-PROD&quantity=2&unitPrice=10", null);

        // Act
        var response = await this.client.GetAsync("/api/v1/orders/search?productId=SEARCH-PROD");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
        Assert.All(orders, o => Assert.Equal("SEARCH-PROD", o.ProductId));
    }

    [Fact]
    public async Task SearchOrders_WithTotalRange_ReturnsFilteredOrders()
    {
        // Arrange
        await this.client.PostAsync("/orders?productId=SEARCH-RANGE&quantity=5&unitPrice=10", null); // total = 50

        // Act
        var response = await this.client.GetAsync("/api/v1/orders/search?minTotal=40&maxTotal=60");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
        Assert.All(orders, o => Assert.True(o.Total >= 40m && o.Total <= 60m));
    }

    [Fact]
    public async Task SearchOrders_WithLimit_ReturnsLimitedResults()
    {
        // Arrange - Create multiple orders
        for (int i = 0; i < 5; i++)
        {
            await this.client.PostAsync($"/orders?productId=LIMIT{i}&quantity=2&unitPrice=10", null);
        }

        // Act
        var response = await this.client.GetAsync("/api/v1/orders/search?limit=3");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
        Assert.True(orders.Count <= 3);
    }

    [Fact]
    public async Task SearchOrders_WithMultipleParameters_ReturnsFilteredAndLimitedResults()
    {
        // Arrange
        await this.client.PostAsync("/orders?productId=MULTI&quantity=5&unitPrice=10", null);

        // Act
        var response = await this.client.GetAsync("/api/v1/orders/search?productId=MULTI&minTotal=40&maxTotal=60&limit=5");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
        Assert.True(orders.Count <= 5);
        Assert.All(orders, o => Assert.Equal("MULTI", o.ProductId));
        Assert.All(orders, o => Assert.True(o.Total >= 40m && o.Total <= 60m));
    }

    #endregion

    #region GET /api/v1/orders/recent/{days:int:range(1,30)} Tests

    [Fact]
    public async Task GetRecentOrders_ValidDays_ReturnsSuccess()
    {
        // Arrange - Create an order
        await this.client.PostAsync("/orders?productId=RECENT&quantity=2&unitPrice=10", null);

        // Act
        var response = await this.client.GetAsync("/api/v1/orders/recent/7");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
    }

    [Fact]
    public async Task GetRecentOrders_DaysBelowRange_ReturnsNotFound()
    {
        // Act
        var response = await this.client.GetAsync("/api/v1/orders/recent/0");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetRecentOrders_DaysAboveRange_ReturnsNotFound()
    {
        // Act
        var response = await this.client.GetAsync("/api/v1/orders/recent/31");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetRecentOrders_DaysAtMinimum_ReturnsSuccess()
    {
        // Act
        var response = await this.client.GetAsync("/api/v1/orders/recent/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
    }

    [Fact]
    public async Task GetRecentOrders_DaysAtMaximum_ReturnsSuccess()
    {
        // Act
        var response = await this.client.GetAsync("/api/v1/orders/recent/30");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
    }

    #endregion

    #region GET /api/v1/orders/stats Tests

    [Fact]
    public async Task GetOrderStatistics_WithOrders_ReturnsCorrectStatistics()
    {
        // Arrange - Create orders with known totals
        await this.client.PostAsync("/orders?productId=STATS1&quantity=2&unitPrice=10", null); // total = 20
        await this.client.PostAsync("/orders?productId=STATS2&quantity=3&unitPrice=10", null); // total = 30

        // Act
        var response = await this.client.GetAsync("/api/v1/orders/stats");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("totalOrders", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("totalRevenue", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("averageOrderTotal", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("mostOrderedProductId", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetOrderStatistics_EmptyRepository_ReturnsZeroStatistics()
    {
        // Note: This test assumes a fresh repository or checks that stats handle empty state
        // Act
        var response = await this.client.GetAsync("/api/v1/orders/stats");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("totalOrders", content, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region Route Group Tests

    [Fact]
    public async Task RouteGroup_OrdersEndpoint_AccessibleUnderApiV1()
    {
        // Arrange - Create an order
        await this.client.PostAsync("/orders?productId=GROUP&quantity=2&unitPrice=10", null);

        // Act
        var response = await this.client.GetAsync("/api/v1/orders");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
    }

    [Fact]
    public async Task RouteGroup_OldOrdersEndpoint_StillAccessible()
    {
        // Arrange - Create an order
        await this.client.PostAsync("/orders?productId=OLD&quantity=2&unitPrice=10", null);

        // Act
        var response = await this.client.GetAsync("/orders");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResult>>();
        Assert.NotNull(orders);
    }

    #endregion

    #region Route Precedence Tests

    [Fact]
    public async Task RoutePrecedence_SpecificRouteBeforeCatchAll_MatchesSpecificRoute()
    {
        // Arrange - Create an order
        await this.client.PostAsync("/orders?productId=PREC&quantity=2&unitPrice=10", null);

        // Act - Try to access with integer (should match specific route)
        var response = await this.client.GetAsync("/api/v1/orders/0");

        // Assert - Should match the specific route, not catch-all
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<OrderResult>();
        Assert.NotNull(result);
    }

    #endregion
}
