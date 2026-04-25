using Microsoft.Extensions.Logging.Abstractions;
using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Models.Exceptions;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Order processing service.
/// TODO: Add structured logging using ILogger&lt;OrderService&gt;.
/// 
/// Tasks:
/// 1. Add constructor parameter: ILogger&lt;OrderService&gt; logger
/// 2. Store logger in a private readonly field
/// 3. Add logging in ProcessOrderAsync:
///    - Log Information when order processing starts: "Processing order for product {ProductId} with quantity {Quantity}"
///    - Log Warning when validation fails: "Order validation failed: {ErrorMessage}"
///    - Log Information when order is successfully processed: "Order processed successfully. ProductId: {ProductId}, Total: {Total}"
///    - Log Error when exception occurs: "Error processing order: {ErrorMessage}"
/// 
/// Hint: Use structured logging with parameters: logger.LogInformation("Processing order for product {ProductId} with quantity {Quantity}", productId, quantity)
/// </summary>
public class OrderService : IOrderService
{
    private readonly IPricingService pricingService;
    private readonly IOrderRepository repository;
    private readonly IOrderValidator validator;
    private readonly ILogger<OrderService> logger;

    /// <summary>
    /// Initializes a new instance of the OrderService class.
    /// </summary>
    /// <param name="pricingService">The pricing service for calculating order totals.</param>
    /// <param name="repository">The repository for saving orders.</param>
    /// <param name="validator">The validator for validating order parameters.</param>
    /// <param name="logger">The logger instance.</param>
    public OrderService(IPricingService pricingService, IOrderRepository repository, IOrderValidator validator)
        : this(pricingService, repository, validator, NullLogger<OrderService>.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the OrderService class.
    /// </summary>
    /// <param name="pricingService">The pricing service for calculating order totals.</param>
    /// <param name="repository">The repository for saving orders.</param>
    /// <param name="validator">The validator for validating order parameters.</param>
    /// <param name="logger">The logger instance.</param>
    public OrderService(IPricingService pricingService, IOrderRepository repository, IOrderValidator validator, ILogger<OrderService> logger)
    {
        this.pricingService = pricingService;
        this.repository = repository;
        this.validator = validator;
        this.logger = logger;
    }

    /// <summary>
    /// Processes an order asynchronously.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="quantity">The order quantity.</param>
    /// <param name="unitPrice">The price per unit.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the order result.</returns>
    /// <exception cref="ArgumentException">Thrown when order validation fails.</exception>
    public async Task<OrderResult> ProcessOrderAsync(string productId, int quantity, decimal unitPrice)
    {
        this.logger.LogInformation("Processing order for product {ProductId} with quantity {Quantity}", productId, quantity);

        try
        {
            this.ValidateOrder(productId, quantity, unitPrice);

            var total = this.pricingService.CalculateTotal(unitPrice, quantity);
            var result = new OrderResult(productId, quantity, total);

            await this.repository.SaveAsync(result);

            this.logger.LogInformation("Order processed successfully. ProductId: {ProductId}, Total: {Total}", productId, total);

            return result;
        }
        catch (InvalidOrderException ex)
        {
            this.logger.LogWarning("Order validation failed: {ErrorMessage}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            this.logger.LogError("Error processing order: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    private void ValidateOrder(string productId, int quantity, decimal unitPrice)
    {
        var validationResult = this.validator.Validate(productId, quantity, unitPrice);

        if (!validationResult.IsValid)
        {
            throw new InvalidOrderException(validationResult.ErrorMessage ?? "Order validation failed");
        }
    }
}
