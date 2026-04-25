using OrderProcessingWithDi.Models;

namespace OrderProcessingWithDi.Services.Interfaces;

/// <summary>
/// Repository interface for order data access.
/// Demonstrates Singleton lifetime pattern.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Saves an order result asynchronously.
    /// </summary>
    /// <param name="order">The order result to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveAsync(OrderResult order);
    
    /// <summary>
    /// Gets all saved order results.
    /// </summary>
    /// <returns>A read-only list of all order results.</returns>
    IReadOnlyList<OrderResult> GetAll();
    
    /// <summary>
    /// Gets an order result by its ID.
    /// TODO: Implement this method for routing task.
    /// </summary>
    /// <param name="id">The order ID.</param>
    /// <returns>The order result if found, null otherwise.</returns>
    OrderResult? GetById(int id);
    
    /// <summary>
    /// Clears all orders from the repository.
    /// This method is primarily for testing purposes.
    /// </summary>
    void Clear();
}

