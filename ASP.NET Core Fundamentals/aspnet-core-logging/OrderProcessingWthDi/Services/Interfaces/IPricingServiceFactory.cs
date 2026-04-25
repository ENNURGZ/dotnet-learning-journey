namespace OrderProcessingWithDi.Services.Interfaces;

/// <summary>
/// Factory interface for creating pricing services.
/// Demonstrates Factory Pattern with DI.
/// </summary>
public interface IPricingServiceFactory
{
    /// <summary>
    /// Creates a pricing service based on the provided type.
    /// </summary>
    /// <param name="serviceType">The type of pricing service to create. Defaults to "standard" if null.</param>
    /// <returns>An instance of IPricingService.</returns>
    /// <exception cref="ArgumentException">Thrown when an unknown service type is provided.</exception>
    IPricingService CreatePricingService(string? serviceType = null);
}

