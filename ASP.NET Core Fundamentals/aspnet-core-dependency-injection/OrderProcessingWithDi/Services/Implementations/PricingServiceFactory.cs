using Microsoft.Extensions.DependencyInjection;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Factory implementation for creating pricing services.
/// </summary>
public class PricingServiceFactory : IPricingServiceFactory
{
    private readonly IServiceProvider _serviceProvider;
    
    public PricingServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IPricingService CreatePricingService(string? serviceType = null)
    {
        serviceType ??= "standard";

        return serviceType switch
        {
            "standard" => _serviceProvider.GetRequiredService<IPricingService>(),
            "simple" => new SimplePricingService(),
            _ => throw new ArgumentException($"Unknown pricing service type: {serviceType}")
        };
    }
}
