using OrderProcessingWithDi.Models.Configuration;
using OrderProcessingWithDi.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Standard pricing service with discount support.
/// TODO: Update to use IOptions&lt;PricingOptions&gt; instead of hardcoded constants.
/// Tasks:
/// 1. Add constructor parameter: IOptions&lt;PricingOptions&gt; options
/// 2. Store options.Value in a private readonly field
/// 3. Replace hardcoded constants with values from options:
///    - DiscountThreshold → options.DiscountThreshold
///    - DiscountPercentage → options.DiscountPercentage
///    - MinimumOrderValue → options.MinimumOrderValue
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
            total *= (1 - this.options.DiscountPercentage);
        }

        return total;
    }
}
