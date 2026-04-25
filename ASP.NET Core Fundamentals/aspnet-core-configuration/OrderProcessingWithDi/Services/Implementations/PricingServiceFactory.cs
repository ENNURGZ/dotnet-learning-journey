using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Factory implementation for creating pricing services.
/// TODO: Implement factory for creating pricing services.
/// 
/// Tasks:
/// 1. Add field to store IServiceProvider and constructor for its injection
/// 
/// 2. Implement the CreatePricingService method:
///    - If serviceType is null, set default value to "standard"
///    - Use switch expression to choose implementation:
///      * "standard" → get IPricingService from serviceProvider.GetRequiredService<IPricingService>()
///      * "simple" → create new instance of SimplePricingService()
///      * for unknown type → throw ArgumentException with message "Unknown pricing service type: {serviceType}"
/// 
/// Hint: IServiceProvider is injected through constructor (DI)
/// </summary>
public class PricingServiceFactory : IPricingServiceFactory
{
    private readonly IServiceProvider serviceProvider;

    public PricingServiceFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IPricingService CreatePricingService(string? serviceType = null)
    {
        serviceType ??= "standard";

        return serviceType.ToLowerInvariant() switch
        {
            "standard" => this.serviceProvider.GetRequiredService<IPricingService>(),
            "simple" => new SimplePricingService(),
            _ => throw new ArgumentException($"Unknown pricing service type: {serviceType}")
        };
    }
}

