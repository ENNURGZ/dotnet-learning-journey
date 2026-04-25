using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Simple pricing service without discounts.
/// Demonstrates multiple implementations of the same interface.
/// </summary>
public class SimplePricingService : IPricingService
{
    /// <summary>
    /// Calculates the total price by multiplying base price by quantity.
    /// No discounts are applied.
    /// </summary>
    /// <param name="basePrice">The base price per unit.</param>
    /// <param name="quantity">The quantity of items.</param>
    /// <returns>The calculated total price (basePrice * quantity).</returns>
    public decimal CalculateTotal(decimal basePrice, int quantity)
    {
        return basePrice * quantity;
    }
}

