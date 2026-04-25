using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Order validation service.
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
