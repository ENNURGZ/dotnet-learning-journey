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

    /// <summary>
    /// Gets an order result by its ID (index-based).
    /// TODO: Implement this method for routing task.
    /// The ID should be the 0-based index in the orders list.
    /// </summary>
    /// <param name="id">The order ID (0-based index).</param>
    /// <returns>The order result if found, null otherwise.</returns>
    public OrderResult? GetById(int id)
    {
        if (id < 0 || id >= orders.Count)
        {
            return null;
        }

        return orders[id];
    }

    /// <summary>
    /// Clears all orders from the repository.
    /// This method is primarily for testing purposes.
    /// </summary>
    public void Clear()
    {
        orders.Clear();
    }
}

