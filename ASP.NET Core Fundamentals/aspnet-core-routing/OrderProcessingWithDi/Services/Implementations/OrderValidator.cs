using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Order validation service.
/// TODO: Implement order validation.
/// 
/// Tasks:
/// Implement the Validate method that checks:
/// 1. productId must not be null or empty string (use string.IsNullOrWhiteSpace)
///    - If invalid → return (false, "ProductId cannot be empty")
/// 
/// 2. quantity must be greater than 0
///    - If invalid → return (false, "Quantity must be greater than 0")
/// 
/// 3. unitPrice must be greater than 0
///    - If invalid → return (false, "UnitPrice must be greater than 0")
/// 
/// If all checks pass, return (true, null)
/// </summary>
public class OrderValidator : IOrderValidator
{
    public (bool IsValid, string? ErrorMessage) Validate(string productId, int quantity, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(productId))
        {
            return (false, "ProductId cannot be empty");
        }

        if (quantity <= 0)
        {
            return (false, "Quantity must be greater than 0");
        }

        if (unitPrice <= 0)
        {
            return (false, "UnitPrice must be greater than 0");
        }

        return (true, null);
    }
}

