using Microsoft.Extensions.Configuration;

namespace OrderProcessingWithDi.Tests.Configuration;

/// <summary>
/// Tests for logging configuration in appsettings.json.
/// Verifies that log levels are correctly configured for different namespaces.
/// </summary>
public class LoggingConfigurationTests : IClassFixture<ConfigurationFixture>
{
    private readonly IConfiguration configuration;

    public LoggingConfigurationTests(ConfigurationFixture fixture)
    {
        this.configuration = fixture.Configuration;
    }

    [Fact]
    public void LoggingConfiguration_ContainsDefaultLogLevel()
    {
        // Arrange & Act
        var defaultLogLevel = this.configuration["Logging:LogLevel:Default"];

        // Assert
        Assert.NotNull(defaultLogLevel);
        Assert.Equal("Information", defaultLogLevel);
    }

    [Fact]
    public void LoggingConfiguration_ContainsMicrosoftAspNetCoreLogLevel()
    {
        // Arrange & Act
        var aspNetCoreLogLevel = this.configuration["Logging:LogLevel:Microsoft.AspNetCore"];

        // Assert
        Assert.NotNull(aspNetCoreLogLevel);
        Assert.Equal("Warning", aspNetCoreLogLevel);
    }

    [Fact]
    public void LoggingConfiguration_ContainsOrderServiceLogLevel()
    {
        // Arrange & Act
        var orderServiceLogLevel = this.configuration["Logging:LogLevel:OrderProcessingWithDi.Services.Implementations.OrderService"];

        // Assert
        Assert.NotNull(orderServiceLogLevel);
        Assert.Equal("Debug", orderServiceLogLevel);
    }

    [Fact]
    public void LoggingConfiguration_ContainsPricingServiceLogLevel()
    {
        // Arrange & Act
        var pricingServiceLogLevel = this.configuration["Logging:LogLevel:OrderProcessingWithDi.Services.Implementations.PricingService"];

        // Assert
        Assert.NotNull(pricingServiceLogLevel);
        Assert.Equal("Debug", pricingServiceLogLevel);
    }

    [Fact]
    public void LoggingConfiguration_CanBeDeserialized()
    {
        // Arrange & Act
        var loggingSection = this.configuration.GetSection("Logging:LogLevel");

        // Assert
        Assert.NotNull(loggingSection);
        Assert.True(loggingSection.Exists());

        // Verify all required log levels are present
        var logLevels = loggingSection.GetChildren().ToList();
        Assert.True(logLevels.Count >= 4, "Expected at least 4 log level configurations");

        var keys = logLevels.Select(c => c.Key).ToList();
        Assert.Contains("Default", keys);
        Assert.Contains("Microsoft.AspNetCore", keys);
        Assert.Contains("OrderProcessingWithDi.Services.Implementations.OrderService", keys);
        Assert.Contains("OrderProcessingWithDi.Services.Implementations.PricingService", keys);
    }

    [Fact]
    public void LoggingConfiguration_LogLevelsAreValid()
    {
        // Arrange
        var validLogLevels = new[] { "Trace", "Debug", "Information", "Warning", "Error", "Critical", "None" };

        // Act
        var loggingSection = this.configuration.GetSection("Logging:LogLevel");
        var logLevels = loggingSection.GetChildren().ToList();

        // Assert
        foreach (var logLevelConfig in logLevels)
        {
            var logLevelValue = logLevelConfig.Value;
            Assert.NotNull(logLevelValue);
            Assert.Contains(logLevelValue, validLogLevels, StringComparer.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void LoggingConfiguration_StructureIsCorrect()
    {
        // Arrange & Act
        var loggingSection = this.configuration.GetSection("Logging");
        var logLevelSection = loggingSection.GetSection("LogLevel");

        // Assert
        Assert.True(loggingSection.Exists(), "Logging section should exist");
        Assert.True(logLevelSection.Exists(), "LogLevel section should exist");

        // Verify structure: Logging -> LogLevel -> {namespace: level}
        var logLevels = logLevelSection.GetChildren().ToList();
        Assert.True(logLevels.Count > 0, "LogLevel section should contain at least one configuration");
    }
}

/// <summary>
/// Fixture for sharing IConfiguration instance across tests to improve performance.
/// </summary>
public class ConfigurationFixture : IDisposable
{
    public IConfiguration Configuration { get; }

    public ConfigurationFixture()
    {
        // Get the path to the main project directory
        var currentDirectory = Directory.GetCurrentDirectory();
        var projectRoot = Path.Combine(currentDirectory, "..", "..", "..", "..", "OrderProcessingWthDi");
        
        // If that doesn't work, try alternative path
        if (!Directory.Exists(projectRoot))
        {
            projectRoot = Path.Combine(currentDirectory, "..", "OrderProcessingWthDi");
        }

        var appsettingsPath = Path.Combine(projectRoot, "appsettings.json");
        
        if (!File.Exists(appsettingsPath))
        {
            throw new FileNotFoundException($"appsettings.json not found at {appsettingsPath}");
        }

        var builder = new ConfigurationBuilder()
            .SetBasePath(projectRoot)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        this.Configuration = builder.Build();
    }

    public void Dispose()
    {
        // Configuration doesn't need explicit disposal
    }
}
