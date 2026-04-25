using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging.Abstractions;
using OrderProcessingWithDi.Models.Configuration;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Standard pricing service with discount support using configuration.
/// TODO: Add structured logging using ILogger&lt;PricingService&gt;.
/// 
/// Tasks:
/// 1. Add constructor parameter: ILogger&lt;PricingService&gt; logger
/// 2. Store logger in a private readonly field
/// 3. Add logging in CalculateTotal:
///    - Log Debug when calculation starts: "Calculating total for price {Price} and quantity {Quantity}"
///    - Log Information when discount is applied: "Discount applied. Original total: {OriginalTotal}, Discount: {DiscountPercentage}%, Final total: {FinalTotal}"
///    - Log Debug when discount is not applied: "No discount applied. Total: {Total}"
/// </summary>
public class PricingService : IPricingService
{
    private readonly PricingOptions options;
    // TODO: Add private readonly field for ILogger<PricingService>
    private readonly ILogger<PricingService> logger;

    /// <summary>
    /// Initializes a new instance of the PricingService class.
    /// </summary>
    /// <param name="options">The pricing configuration options.</param>
    /// <summary>
    /// Initializes a new instance of the PricingService class.
    /// </summary>
    /// <param name="options">The pricing configuration options.</param>
    public PricingService(IOptions<PricingOptions> options)
        : this(options, NullLogger<PricingService>.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the PricingService class.
    /// </summary>
    /// <param name="options">The pricing configuration options.</param>
    /// <param name="logger">The logger instance.</param>
    public PricingService(IOptions<PricingOptions> options, ILogger<PricingService> logger)
    {
        this.options = options.Value;
        this.logger = logger;
    }

    /// <summary>
    /// Calculates the total price for an order with discount support.
    /// Applies a discount if quantity exceeds the discount threshold from configuration.
    /// </summary>
    /// <param name="basePrice">The base price per unit.</param>
    /// <param name="quantity">The quantity of items.</param>
    /// <returns>The calculated total price after applying discounts if applicable.</returns>
    public decimal CalculateTotal(decimal basePrice, int quantity)
    {
        this.logger.LogDebug("Calculating total for price {Price} and quantity {Quantity}", basePrice, quantity);

        var total = basePrice * quantity;

        if (quantity > this.options.DiscountThreshold && total >= this.options.MinimumOrderValue)
        {
            var originalTotal = total;
            total *= (1 - this.options.DiscountPercentage);
            
            this.logger.LogInformation("Discount applied. Original total: {OriginalTotal}, Discount: {DiscountPercentage}%, Final total: {FinalTotal}", 
                originalTotal, this.options.DiscountPercentage * 100, total);
        }
        else
        {
            this.logger.LogDebug("No discount applied. Total: {Total}", total);
        }

        return total;
    }
}
