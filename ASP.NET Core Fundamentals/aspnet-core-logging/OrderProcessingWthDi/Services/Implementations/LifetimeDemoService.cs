using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// Implementation demonstrating different lifetimes.
/// This service will be registered with different lifetimes to show the difference.
/// </summary>
public class LifetimeDemoService : ILifetimeDemoService
{
    private static int instanceCounter = 0;

    /// <summary>
    /// Gets the unique instance ID for this service instance.
    /// </summary>
    public string InstanceId { get; }

    /// <summary>
    /// Gets the creation timestamp in UTC.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Initializes a new instance of the LifetimeDemoService class.
    /// Assigns a unique instance ID and creation timestamp.
    /// </summary>
    public LifetimeDemoService()
    {
        this.InstanceId = $"Instance-{Interlocked.Increment(ref instanceCounter)}";
        this.CreatedAt = DateTime.UtcNow;
    }
}

