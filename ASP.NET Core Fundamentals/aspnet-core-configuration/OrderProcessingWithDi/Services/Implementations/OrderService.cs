using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Models.Exceptions;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Order processing service.
/// TODO: Implement this class using Dependency Injection.
/// 
/// Tasks:
/// 1. Add constructor with dependency injection:
///    - IPricingService (for calculating total price)
///    - IOrderRepository (for saving orders)
///    - IOrderValidator (for order validation)
/// 
/// 2. Implement the ProcessOrderAsync method:
///    - Validate order using validator.Validate()
///    - If validation fails, throw InvalidOrderException with error message
///    - Calculate total using pricingService.CalculateTotal()
///    - Create OrderResult with productId, quantity, and calculated total
///    - Save result via repository.SaveAsync()
///    - Return OrderResult
/// 
/// Hint: Use constructor injection for dependencies (Constructor Injection)
/// </summary>
public class OrderService : IOrderService
{
    private readonly IPricingService pricingService;
    private readonly IOrderRepository repository;
    private readonly IOrderValidator validator;

    public OrderService(IPricingService pricingService, IOrderRepository repository, IOrderValidator validator)
    {
        this.pricingService = pricingService;
        this.repository = repository;
        this.validator = validator;
    }

    public async Task<OrderResult> ProcessOrderAsync(string productId, int quantity, decimal unitPrice)
    {
        this.ValidateOrder(productId, quantity, unitPrice);

        var total = this.pricingService.CalculateTotal(unitPrice, quantity);
        var orderResult = new OrderResult(productId, quantity, total);

        await this.repository.SaveAsync(orderResult);

        return orderResult;
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

