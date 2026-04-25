namespace OrderProcessingWithDi.Services.Interfaces;

/// <summary>
/// Service to demonstrate different DI lifetimes (Singleton, Scoped, Transient).
/// Each request will show different instance IDs based on lifetime.
/// </summary>
public interface ILifetimeDemoService
{
    string InstanceId { get; }
    DateTime CreatedAt { get; }
}

public interface ISingletonService : ILifetimeDemoService { }
public interface IScopedService : ILifetimeDemoService { }
public interface ITransientService : ILifetimeDemoService { }

