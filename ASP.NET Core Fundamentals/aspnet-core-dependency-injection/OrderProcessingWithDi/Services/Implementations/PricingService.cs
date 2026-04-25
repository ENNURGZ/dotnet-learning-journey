using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Standard pricing service with discount support.
/// </summary>
public class PricingService : IPricingService
{
    private const int DiscountThreshold = 5;
    private const decimal DiscountPercentage = 0.1m;
    private const decimal MinimumOrderValue = 0m;

    public decimal CalculateTotal(decimal basePrice, int quantity)
    {
        decimal total = basePrice * quantity;
        
        if (quantity > DiscountThreshold && total >= MinimumOrderValue)
        {
            total *= (1 - DiscountPercentage);
        }

        return total;
    }
}
