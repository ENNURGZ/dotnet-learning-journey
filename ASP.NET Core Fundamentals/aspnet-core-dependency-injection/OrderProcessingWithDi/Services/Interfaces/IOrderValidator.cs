namespace OrderProcessingWithDi.Services.Interfaces;

/// <summary>
/// Service for validating orders.
/// Demonstrates separation of concerns with DI.
/// </summary>
public interface IOrderValidator
{
    /// <summary>
    /// Validates order parameters
    /// </summary>
    /// <returns>Validation result with error message if invalid</returns>
    (bool IsValid, string? ErrorMessage) Validate(string productId, int quantity, decimal unitPrice);
}
