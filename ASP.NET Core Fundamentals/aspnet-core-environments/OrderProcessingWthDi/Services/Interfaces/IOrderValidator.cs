namespace OrderProcessingWithDi.Services.Interfaces;

/// <summary>
/// Service for validating orders.
/// Demonstrates separation of concerns with DI.
/// </summary>
public interface IOrderValidator
{
    /// <summary>
    /// Validates order parameters.
    /// </summary>
    /// <param name="productId">The product identifier to validate.</param>
    /// <param name="quantity">The order quantity to validate.</param>
    /// <param name="unitPrice">The price per unit to validate.</param>
    /// <returns>A tuple containing validation result (IsValid) and error message if invalid.</returns>
    (bool IsValid, string? ErrorMessage) Validate(string productId, int quantity, decimal unitPrice);
}
