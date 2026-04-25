namespace OrderProcessingWithDi.Models.Configuration;

/// <summary>
/// Configuration options for pricing settings.
/// TODO: Create configuration class with properties:
/// - DiscountThreshold (int, default: 5)
/// - DiscountPercentage (decimal, default: 0.1m)
/// - MinimumOrderValue (decimal, default: 0m)
/// TODO: Add SectionName constant with value "Pricing"
/// </summary>
public class PricingOptions
{
    public const string SectionName = "Pricing";

    public int DiscountThreshold { get; set; } = 5;

    public decimal DiscountPercentage { get; set; } = 0.1m;

    public decimal MinimumOrderValue { get; set; } = 0m;
}
