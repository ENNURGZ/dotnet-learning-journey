using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Standard pricing service with discount support.
/// TODO: Implement price calculation with discount.
/// Tasks:
/// 1. Define constants:
///    - DiscountThreshold = 5 (quantity threshold for discount)
///    - DiscountPercentage = 0.1m (10% discount)
///    - MinimumOrderValue = 0m (minimum order value to apply discount)
/// 2. Implement the CalculateTotal method:
///    - Calculate base total: basePrice * quantity
///    - If quantity > DiscountThreshold AND total >= MinimumOrderValue:
///      apply discount: total *= (1 - DiscountPercentage)
///    - Return the final total
/// Example: basePrice=10, quantity=6
/// - Base total: 10 * 6 = 60
/// - quantity (6) > DiscountThreshold (5) → apply discount
/// - Final total: 60 * (1 - 0.1) = 54
/// </summary>
public class PricingService : IPricingService
{
    private const int DiscountThreshold = 5;
    private const decimal DiscountPercentage = 0.1m;
    private const decimal MinimumOrderValue = 0m;

    public decimal CalculateTotal(decimal basePrice, int quantity)
    {
        var total = basePrice * quantity;
        if (quantity > DiscountThreshold && total >= MinimumOrderValue)
        {
            total *= (1 - DiscountPercentage);
        }

        return total;
    }
}

