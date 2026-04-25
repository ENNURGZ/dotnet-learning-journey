using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderProcessingWithDi.Models;
using Xunit;

namespace OrderProcessingWithDi.Tests;

/// <summary>
/// Integration tests for Error Handling functionality.
/// Tests exception handling middleware and error response formats.
/// </summary>
public class ErrorHandlingTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client = factory.CreateClient();

    #region OrderNotFoundException Tests

    [Fact]
    public async Task GetOrderById_NonExistentOrder_Returns404WithErrorResponse()
    {
        // Act
        var response = await this.client.GetAsync("/api/v1/orders/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(errorResponse);
        Assert.Equal(404, errorResponse.Status);
        Assert.Equal("Order Not Found", errorResponse.Title);
        Assert.Contains("99999", errorResponse.Detail);
        Assert.Equal("/api/v1/orders/99999", errorResponse.Instance);
        Assert.True(errorResponse.Extensions.ContainsKey("orderId"));
        // Handle JSON number deserialization (may be JsonElement, int, or long)
        var orderIdValue = errorResponse.Extensions["orderId"];
        var orderIdInt = orderIdValue switch
        {
            int i => i,
            long l => (int)l,
            System.Text.Json.JsonElement jsonElement => jsonElement.GetInt32(),
            _ => Convert.ToInt32(orderIdValue)
        };
        Assert.Equal(99999, orderIdInt);
    }

    [Fact]
    public async Task GetOrderById_NonExistentOrder_ReturnsProblemDetailsFormat()
    {
        // Act
        var response = await this.client.GetAsync("/api/v1/orders/12345");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(errorResponse);
        Assert.Contains("rfc7231#section-6.5.4", errorResponse.Type);
        Assert.Equal("Order Not Found", errorResponse.Title);
        Assert.Equal(404, errorResponse.Status);
    }

    #endregion

    #region InvalidOrderException Tests

    [Fact]
    public async Task PostOrder_EmptyProductId_Returns400WithErrorResponse()
    {
        // Act
        var response = await this.client.PostAsync("/orders?productId=&quantity=5&unitPrice=10", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(errorResponse);
        Assert.Equal(400, errorResponse.Status);
        Assert.Equal("Invalid Order", errorResponse.Title);
        Assert.Contains("ProductId cannot be empty", errorResponse.Detail);
        Assert.Equal("/orders", errorResponse.Instance);
    }

    [Fact]
    public async Task PostOrder_ZeroQuantity_Returns400WithErrorResponse()
    {
        // Act
        var response = await this.client.PostAsync("/orders?productId=A1&quantity=0&unitPrice=10", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(errorResponse);
        Assert.Equal(400, errorResponse.Status);
        Assert.Equal("Invalid Order", errorResponse.Title);
        Assert.Contains("Quantity must be greater than 0", errorResponse.Detail);
    }

    [Fact]
    public async Task PostOrder_NegativeUnitPrice_Returns400WithErrorResponse()
    {
        // Act
        var response = await this.client.PostAsync("/orders?productId=A1&quantity=5&unitPrice=-10", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(errorResponse);
        Assert.Equal(400, errorResponse.Status);
        Assert.Equal("Invalid Order", errorResponse.Title);
        Assert.Contains("UnitPrice must be greater than 0", errorResponse.Detail);
    }

    [Fact]
    public async Task PostOrder_InvalidOrder_ReturnsProblemDetailsFormat()
    {
        // Act
        var response = await this.client.PostAsync("/orders?productId=&quantity=5&unitPrice=10", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(errorResponse);
        Assert.Contains("rfc7231#section-6.5.1", errorResponse.Type);
        Assert.Equal("Invalid Order", errorResponse.Title);
        Assert.Equal(400, errorResponse.Status);
    }

    #endregion

    #region ArgumentException Tests

    [Fact]
    public async Task PostOrder_InvalidArgument_Returns400WithErrorResponse()
    {
        // Act - This should trigger InvalidOrderException from validation
        var response = await this.client.PostAsync("/orders?productId=TEST&quantity=-1&unitPrice=10", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(errorResponse);
        Assert.Equal(400, errorResponse.Status);
        Assert.Equal("Invalid Order", errorResponse.Title);
    }

    #endregion

    #region Error Response Format Tests

    [Fact]
    public async Task ErrorResponse_HasRequiredFields()
    {
        // Act
        var response = await this.client.GetAsync("/api/v1/orders/99999");

        // Assert
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(errorResponse);
        Assert.False(string.IsNullOrEmpty(errorResponse.Type));
        Assert.False(string.IsNullOrEmpty(errorResponse.Title));
        Assert.True(errorResponse.Status > 0);
        Assert.False(string.IsNullOrEmpty(errorResponse.Detail));
        Assert.False(string.IsNullOrEmpty(errorResponse.Instance));
    }

    [Fact]
    public async Task ErrorResponse_UsesCamelCase()
    {
        // Act
        var response = await this.client.GetAsync("/api/v1/orders/99999");

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("orderId", content); // camelCase
        Assert.DoesNotContain("OrderId", content); // PascalCase should not be present
    }

    #endregion

    #region Middleware Order Tests

    [Fact]
    public async Task ErrorHandling_WorksForAllEndpoints()
    {
        // Test that error handling works for endpoints that throw exceptions
        var endpoints = new[]
        {
            "/api/v1/orders/99999",
            "/orders/99999",
        };

        foreach (var endpoint in endpoints)
        {
            var response = await this.client.GetAsync(endpoint);
            Assert.True(response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest);
            
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(content))
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                Assert.NotNull(errorResponse);
                Assert.True(errorResponse.Status >= 400);
            }
        }
        
        // Test constraint violation endpoint (returns 404 without JSON body from routing, not our middleware)
        var constraintResponse = await this.client.GetAsync("/api/v1/orders/product/");
        Assert.Equal(HttpStatusCode.NotFound, constraintResponse.StatusCode);
    }

    #endregion
}

