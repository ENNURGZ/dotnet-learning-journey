namespace OrderProcessingWithDi.Models.Configuration;

/// <summary>
/// Configuration options for pricing settings.
/// </summary>
public class PricingOptions
{
    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public const string SectionName = "Pricing";

    /// <summary>
    /// Gets or sets the quantity threshold for applying discount.
    /// </summary>
    public int DiscountThreshold { get; set; } = 5;

    /// <summary>
    /// Gets or sets the discount percentage (0.1 = 10%).
    /// </summary>
    public decimal DiscountPercentage { get; set; } = 0.1m;

    /// <summary>
    /// Gets or sets the minimum order value to apply discount.
    /// </summary>
    public decimal MinimumOrderValue { get; set; } = 0m;
}
