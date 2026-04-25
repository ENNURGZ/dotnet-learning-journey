using Microsoft.Extensions.Logging.Abstractions;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Order validation service.
/// TODO: Add structured logging using ILogger&lt;OrderValidator&gt;.
///
/// Tasks:
/// 1. Add constructor parameter: ILogger&lt;OrderValidator&gt; logger
/// 2. Store logger in a private readonly field
/// 3. Add logging in Validate:
///    - Log Debug when validation starts: "Validating order: ProductId={ProductId}, Quantity={Quantity}, UnitPrice={UnitPrice}"
///    - Log Warning when validation fails: "Validation failed: {ValidationMessage}"
///    - Log Debug when validation succeeds: "Validation passed for product {ProductId}"
/// </summary>
public class OrderValidator : IOrderValidator
{
    // TODO: Add private readonly field for ILogger<OrderValidator>
    private readonly ILogger<OrderValidator> logger;

    // TODO: Add constructor with ILogger<OrderValidator> parameter
    /// <summary>
    /// Initializes a new instance of the OrderValidator class.
    /// </summary>
    public OrderValidator()
        : this(NullLogger<OrderValidator>.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the OrderValidator class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public OrderValidator(ILogger<OrderValidator> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Validates order parameters.
    /// </summary>
    /// <param name="productId">The product identifier to validate.</param>
    /// <param name="quantity">The order quantity to validate.</param>
    /// <param name="unitPrice">The price per unit to validate.</param>
    /// <returns>A tuple containing validation result (IsValid) and error message if invalid.</returns>
    public (bool IsValid, string? ErrorMessage) Validate(string productId, int quantity, decimal unitPrice)
    {
        this.logger.LogDebug("Validating order: ProductId={ProductId}, Quantity={Quantity}, UnitPrice={UnitPrice}", productId, quantity, unitPrice);

        if (string.IsNullOrWhiteSpace(productId))
        {
            var message = "ProductId cannot be empty";
            this.logger.LogWarning("Validation failed: {ValidationMessage}", message);
            return (false, message);
        }

        if (quantity <= 0)
        {
            var message = "Quantity must be greater than 0";
            this.logger.LogWarning("Validation failed: {ValidationMessage}", message);
            return (false, message);
        }

        if (unitPrice <= 0)
        {
            var message = "UnitPrice must be greater than 0";
            this.logger.LogWarning("Validation failed: {ValidationMessage}", message);
            return (false, message);
        }

        this.logger.LogDebug("Validation passed for product {ProductId}", productId);
        return (true, null);
    }
}

