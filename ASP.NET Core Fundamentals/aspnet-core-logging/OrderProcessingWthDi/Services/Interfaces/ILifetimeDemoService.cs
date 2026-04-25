namespace OrderProcessingWithDi.Services.Interfaces;

/// <summary>
/// Service to demonstrate different DI lifetimes (Singleton, Scoped, Transient).
/// Each request will show different instance IDs based on lifetime.
/// </summary>
public interface ILifetimeDemoService
{
    /// <summary>
    /// Gets the unique instance ID for this service instance
    /// </summary>
    string InstanceId { get; }
    
    /// <summary>
    /// Gets the creation timestamp
    /// </summary>
    DateTime CreatedAt { get; }
}

