using OrderProcessingWithDi.Models;

namespace OrderProcessingWithDi.Services.Interfaces;

/// <summary>
/// Service interface for processing orders.
/// Demonstrates multiple dependencies injection (Constructor Injection).
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Processes an order asynchronously.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="quantity">The order quantity.</param>
    /// <param name="unitPrice">The price per unit.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the order result.</returns>
    /// <exception cref="ArgumentException">Thrown when order validation fails.</exception>
    Task<OrderResult> ProcessOrderAsync(string productId, int quantity, decimal unitPrice);
}

