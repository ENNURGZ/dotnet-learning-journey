using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Order processing service.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IPricingService _pricingService;
    private readonly IOrderRepository _repository;
    private readonly IOrderValidator _validator;

    public OrderService(IPricingService pricingService, IOrderRepository repository, IOrderValidator validator)
    {
        _pricingService = pricingService;
        _repository = repository;
        _validator = validator;
    }
    
    public async Task<OrderResult> ProcessOrderAsync(string productId, int quantity, decimal unitPrice)
    {
        var (isValid, errorMessage) = _validator.Validate(productId, quantity, unitPrice);
        if (!isValid)
        {
            throw new ArgumentException(errorMessage);
        }

        var total = _pricingService.CalculateTotal(unitPrice, quantity);
        
        var result = new OrderResult(productId, quantity, total);

        await _repository.SaveAsync(result);

        return result;
    }
}