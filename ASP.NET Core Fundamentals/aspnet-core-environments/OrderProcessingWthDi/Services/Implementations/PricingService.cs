using Microsoft.Extensions.Options;
using OrderProcessingWithDi.Models.Configuration;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Standard pricing service with discount support using configuration.
/// TODO: Implement price calculation with discount using configuration.
/// 
/// Tasks:
/// 1. Add constructor parameter: IOptions&lt;PricingOptions&gt; options
/// 2. Store options.Value in private readonly field
/// 3. Implement the CalculateTotal method:
///    - Calculate base total: basePrice * quantity
///    - If quantity > options.DiscountThreshold AND total >= options.MinimumOrderValue:
///      apply discount: total *= (1 - options.DiscountPercentage)
///    - Return the final total
/// 
/// Hint: Use IOptions&lt;PricingOptions&gt; to access configuration values
/// </summary>
public class PricingService : IPricingService
{
    private readonly PricingOptions options;
    
    public PricingService(IOptions<PricingOptions> options)
    {
        this.options = options.Value;
    }

    public decimal CalculateTotal(decimal basePrice, int quantity)
    {
        var total = basePrice * quantity;

        if (quantity > this.options.DiscountThreshold && total >= this.options.MinimumOrderValue)
        {
            total *= 1 - this.options.DiscountPercentage;
        }

        return total;
    }
}
