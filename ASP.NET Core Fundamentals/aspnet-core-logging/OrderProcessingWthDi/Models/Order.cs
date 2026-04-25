namespace OrderProcessingWithDi.Models;

/// <summary>
/// Represents an order with product information.
/// </summary>
/// <param name="ProductId">The unique identifier of the product.</param>
/// <param name="Quantity">The quantity of items ordered.</param>
/// <param name="UnitPrice">The price per unit.</param>
public record Order(string ProductId, int Quantity, decimal UnitPrice);