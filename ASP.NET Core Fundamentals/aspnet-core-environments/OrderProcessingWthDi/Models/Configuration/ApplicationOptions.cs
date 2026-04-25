namespace OrderProcessingWithDi.Models.Configuration;

/// <summary>
/// Configuration options for application settings.
/// </summary>
public class ApplicationOptions
{
    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public const string SectionName = "Application";

    /// <summary>
    /// Gets or sets the application name.
    /// </summary>
    public string ApplicationName { get; set; } = "Order Processing API";

    /// <summary>
    /// Gets or sets the application version.
    /// </summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// Gets or sets the environment name.
    /// </summary>
    public string Environment { get; set; } = "Development";
}
