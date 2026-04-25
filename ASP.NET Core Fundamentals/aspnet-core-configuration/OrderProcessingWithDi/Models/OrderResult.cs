namespace OrderProcessingWithDi.Models;

/// <summary>
/// Represents the result of processing an order.
/// </summary>
/// <param name="ProductId">The unique identifier of the product.</param>
/// <param name="Quantity">The quantity of items ordered.</param>
/// <param name="Total">The total price after calculations (including discounts if applicable).</param>
public record OrderResult(string ProductId, int Quantity, decimal Total);

