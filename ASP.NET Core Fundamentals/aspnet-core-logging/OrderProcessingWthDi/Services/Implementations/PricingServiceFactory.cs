using Microsoft.Extensions.DependencyInjection;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Factory implementation for creating pricing services.
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
/// Hint: IServiceProvider is injected through constructor (DI).
/// </summary>
public class PricingServiceFactory : IPricingServiceFactory
{
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Initializes a new instance of the PricingServiceFactory class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    public PricingServiceFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Creates a pricing service based on the provided type.
    /// </summary>
    /// <param name="serviceType">The type of pricing service to create. Defaults to "standard" if null.</param>
    /// <returns>An instance of IPricingService.</returns>
    /// <exception cref="ArgumentException">Thrown when an unknown service type is provided.</exception>
    public IPricingService CreatePricingService(string? serviceType = null)
    {
        serviceType ??= "standard";

        return serviceType switch
        {
            "standard" => this.serviceProvider.GetRequiredService<IPricingService>(),
            "simple" => new SimplePricingService(),
            _ => throw new ArgumentException($"Unknown pricing service type: {serviceType}")
        };
    }
}
