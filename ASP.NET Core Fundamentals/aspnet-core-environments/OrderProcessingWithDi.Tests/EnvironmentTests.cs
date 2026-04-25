using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderProcessingWithDi;

namespace OrderProcessingWithDi.Tests;

/// <summary>
/// Integration tests for ASP.NET Core Environments functionality.
/// Tests the /config/environment endpoint that demonstrates IWebHostEnvironment usage.
///
/// These tests are specifically for the OrderProcessingWithDi project,
/// which extends OrderProcessingWithDi with Environments functionality.
/// </summary>
public class EnvironmentTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public EnvironmentTests(WebApplicationFactory<Program> factory)
    {
        this._factory = factory;
        this._client = factory.CreateClient();
    }

    #region GET /config/environment Tests

    [Fact]
    public async Task GetEnvironmentConfig_ReturnsEnvironmentInformation()
    {
        // Act
        var response = await this._client.GetAsync("/config/environment");

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<EnvironmentConfigResponse>();
        Assert.NotNull(result);

        // IWebHostEnvironment properties should be present
        Assert.NotNull(result.EnvironmentName);
        Assert.NotNull(result.ApplicationName);
        Assert.NotNull(result.ContentRootPath);
    }

    [Fact]
    public async Task GetEnvironmentConfig_ReturnsValidEnvironmentName()
    {
        // Act
        var response = await this._client.GetAsync("/config/environment");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<EnvironmentConfigResponse>();
        Assert.NotNull(result);

        // EnvironmentName should be one of the standard values
        Assert.True(
            result.EnvironmentName == "Development" ||
            result.EnvironmentName == "Staging" ||
            result.EnvironmentName == "Production",
            $"EnvironmentName should be Development, Staging, or Production, but was {result.EnvironmentName}");
    }

    [Fact]
    public async Task GetEnvironmentConfig_ReturnsApplicationName()
    {
        // Act
        var response = await this._client.GetAsync("/config/environment");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<EnvironmentConfigResponse>();
        Assert.NotNull(result);

        // ApplicationName should match the assembly name (OrderProcessingWithDi project)
        Assert.Equal("OrderProcessingWithDi", result.ApplicationName);
    }

    [Fact]
    public async Task GetEnvironmentConfig_ReturnsContentRootPath()
    {
        // Act
        var response = await this._client.GetAsync("/config/environment");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<EnvironmentConfigResponse>();
        Assert.NotNull(result);

        // ContentRootPath should be a valid absolute path
        Assert.NotNull(result.ContentRootPath);
        Assert.True(
            Path.IsPathRooted(result.ContentRootPath),
            $"ContentRootPath should be an absolute path, but was {result.ContentRootPath}");
    }

    [Fact]
    public async Task GetEnvironmentConfig_ReturnsWebRootPath()
    {
        // Act
        var response = await this._client.GetAsync("/config/environment");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<EnvironmentConfigResponse>();
        Assert.NotNull(result);

        // WebRootPath can be null if wwwroot doesn't exist, or should be a valid path
        if (result.WebRootPath != null)
        {
            Assert.True(
                Path.IsPathRooted(result.WebRootPath),
                $"WebRootPath should be an absolute path if not null, but was {result.WebRootPath}");
        }
    }

    [Fact]
    public async Task GetEnvironmentConfig_ReturnsPricingConfigurationFromEnv()
    {
        // Act
        var response = await this._client.GetAsync("/config/environment");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<EnvironmentConfigResponse>();
        Assert.NotNull(result);

        // PricingFromEnv should contain the configuration value (can be from appsettings.json or environment variable)
        // The value should be a valid string representation of a number
        Assert.NotNull(result.PricingFromEnv);
    }

    [Fact]
    public async Task GetEnvironmentConfig_ResponseHasCorrectStructure()
    {
        // Act
        var response = await this._client.GetAsync("/config/environment");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<EnvironmentConfigResponse>();
        Assert.NotNull(result);

        // Verify all required properties are present
        Assert.NotNull(result.EnvironmentName);
        Assert.NotNull(result.ApplicationName);
        Assert.NotNull(result.ContentRootPath);
        Assert.NotNull(result.PricingFromEnv);

        // WebRootPath can be null, so we just verify it's either null or a valid path
        if (result.WebRootPath != null)
        {
            Assert.True(Path.IsPathRooted(result.WebRootPath));
        }
    }

    #endregion

    /// <summary>
    /// Response model for /config/environment endpoint.
    /// </summary>
    private class EnvironmentConfigResponse
    {
        public string EnvironmentName { get; set; } = string.Empty;
        public string ApplicationName { get; set; } = string.Empty;
        public string ContentRootPath { get; set; } = string.Empty;
        public string? WebRootPath { get; set; }
        public string? PricingFromEnv { get; set; }
    }
}
