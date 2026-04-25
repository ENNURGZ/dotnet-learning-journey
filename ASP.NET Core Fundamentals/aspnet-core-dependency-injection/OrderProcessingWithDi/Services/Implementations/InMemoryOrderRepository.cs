using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

/// <summary>
/// In-memory order repository.
/// Demonstrates Singleton lifetime - same instance shared across all requests.
/// </summary>
public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<OrderResult> orders = new();

    /// <summary>
    /// Saves an order result asynchronously to the in-memory collection.
    /// </summary>
    /// <param name="order">The order result to save.</param>
    /// <returns>A completed task representing the asynchronous operation.</returns>
    public Task SaveAsync(OrderResult order)
    {
        orders.Add(order);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets all saved order results from the in-memory collection.
    /// </summary>
    /// <returns>A read-only list of all order results.</returns>
    public IReadOnlyList<OrderResult> GetAll() => orders.AsReadOnly();
}