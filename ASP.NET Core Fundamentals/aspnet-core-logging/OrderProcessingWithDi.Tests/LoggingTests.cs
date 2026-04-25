using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace OrderProcessingWithDi.Tests;

/// <summary>
/// Integration tests for ASP.NET Core Logging functionality.
/// Tests the logging demo endpoints that demonstrate different logging patterns.
///
/// These tests are specifically for the OrderProcessingWithDi project,
/// which extends OrderProcessingWithDi with Logging functionality.
/// </summary>
public class LoggingTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory = factory;
    private readonly HttpClient client = factory.CreateClient();

    #region GET /logging/demo Tests

    [Fact]
    public async Task LoggingDemo_ReturnsSuccess()
    {
        // Act
        var response = await this.client.GetAsync("/logging/demo");

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoggingDemoResponse>();
        Assert.NotNull(result);
        Assert.NotNull(result.Message);
        Assert.Contains("Check application logs", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task LoggingDemo_ReturnsCorrectMessage()
    {
        // Act
        var response = await this.client.GetAsync("/logging/demo");

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoggingDemoResponse>();
        Assert.NotNull(result);
        Assert.Equal("Check application logs to see different log levels", result.Message);
    }

    #endregion

    #region GET /logging/structured Tests

    [Fact]
    public async Task StructuredLogging_ReturnsSuccess()
    {
        // Act
        var response = await this.client.GetAsync("/logging/structured");

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<StructuredLoggingResponse>();
        Assert.NotNull(result);
        Assert.NotNull(result.UserId);
        Assert.NotNull(result.Action);
        Assert.True(result.Timestamp > DateTime.MinValue);
    }

    [Fact]
    public async Task StructuredLogging_ReturnsCorrectStructure()
    {
        // Act
        var response = await this.client.GetAsync("/logging/structured");

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<StructuredLoggingResponse>();
        Assert.NotNull(result);
        Assert.Equal("user123", result.UserId);
        Assert.Equal("GetOrders", result.Action);
        Assert.True(result.Timestamp <= DateTime.UtcNow);
    }

    #endregion

    #region GET /logging/scopes Tests

    [Fact]
    public async Task LoggingScopes_ReturnsSuccess()
    {
        // Act
        var response = await this.client.GetAsync("/logging/scopes");

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoggingScopesResponse>();
        Assert.NotNull(result);
        Assert.NotNull(result.Message);
        Assert.Contains("Check logs", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task LoggingScopes_ReturnsCorrectMessage()
    {
        // Act
        var response = await this.client.GetAsync("/logging/scopes");

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoggingScopesResponse>();
        Assert.NotNull(result);
        Assert.Equal("Check logs for scoped logging demonstration", result.Message);
    }

    #endregion

    /// <summary>
    /// Response model for /logging/demo endpoint.
    /// </summary>
    private class LoggingDemoResponse
    {
        public string Message { get; init; } = string.Empty;
    }

    /// <summary>
    /// Response model for /logging/structured endpoint.
    /// </summary>
    private class StructuredLoggingResponse
    {
        public string UserId { get; init; } = string.Empty;
        public string Action { get; init; } = string.Empty;
        public DateTime Timestamp { get; init; }
    }

    /// <summary>
    /// Response model for /logging/scopes endpoint.
    /// </summary>
    private class LoggingScopesResponse
    {
        public string Message { get; init; } = string.Empty;
    }
}
