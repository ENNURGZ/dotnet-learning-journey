namespace OrderProcessingWithDi.Services.Interfaces;

/// <summary>
/// Factory interface for creating pricing services.
/// Demonstrates Factory Pattern with DI.
/// </summary>
public interface IPricingServiceFactory
{
    /// <summary>
    /// Creates a pricing service based on the provided type
    /// </summary>
    IPricingService CreatePricingService(string? serviceType = null);
}

