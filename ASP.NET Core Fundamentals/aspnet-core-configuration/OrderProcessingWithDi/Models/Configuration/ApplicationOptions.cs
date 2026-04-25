namespace OrderProcessingWithDi.Models.Configuration;

/// <summary>
/// Configuration options for application settings.
/// TODO: Create configuration class with properties:
/// - ApplicationName (string, default: "Order Processing API")
/// - Version (string, default: "1.0.0")
/// - Environment (string, default: "Development")
/// TODO: Add SectionName constant with value "Application"
/// </summary>
public class ApplicationOptions
{
    public const string SectionName = "Application";

    public string ApplicationName { get; set; } = "Order Processing API";

    public string Version { get; set; } = "1.0.0";

    public string Environment { get; set; } = "Development";
}
