namespace OrderProcessingWithDi.Models.Configuration;

/// <summary>
/// Configuration options for order processing settings.
/// TODO: Create configuration class with properties:
/// - MaxQuantity (int, default: 1000)
/// - MaxOrderValue (decimal, default: 10000m)
/// - EnableValidation (bool, default: true)
/// TODO: Add SectionName constant with value "OrderProcessing"
/// </summary>
public class OrderProcessingOptions
{
    public const string SectionName = "OrderProcessing";

    public int MaxQuantity { get; set; } = 1000;

    public decimal MaxOrderValue { get; set; } = 10000m;

    public bool EnableValidation { get; set; } = true;
}
