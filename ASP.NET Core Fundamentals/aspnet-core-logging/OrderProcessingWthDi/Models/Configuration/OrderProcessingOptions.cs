namespace OrderProcessingWithDi.Models.Configuration;

/// <summary>
/// Configuration options for order processing settings.
/// </summary>
public class OrderProcessingOptions
{
    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public const string SectionName = "OrderProcessing";

    /// <summary>
    /// Gets or sets the maximum allowed quantity per order.
    /// </summary>
    public int MaxQuantity { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the maximum allowed order value.
    /// </summary>
    public decimal MaxOrderValue { get; set; } = 10000m;

    /// <summary>
    /// Gets or sets a value indicating whether validation is enabled.
    /// </summary>
    public bool EnableValidation { get; set; } = true;
}
