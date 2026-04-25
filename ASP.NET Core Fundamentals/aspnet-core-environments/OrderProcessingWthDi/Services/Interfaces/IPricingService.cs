namespace OrderProcessingWithDi.Services.Interfaces;

/// <summary>
/// Service interface for calculating order totals.
/// Demonstrates Transient lifetime pattern.
/// </summary>
public interface IPricingService
{
    /// <summary>
    /// Calculates the total price for an order.
    /// </summary>
    /// <param name="basePrice">The base price per unit.</param>
    /// <param name="quantity">The quantity of items.</param>
    /// <returns>The calculated total price.</returns>
    decimal CalculateTotal(decimal basePrice, int quantity);
}