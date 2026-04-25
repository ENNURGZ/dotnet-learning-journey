namespace OrderProcessingWithDi.Models.Exceptions;

/// <summary>
/// Exception thrown when an order is not found.
/// </summary>
public class OrderNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the OrderNotFoundException class.
    /// </summary>
    public OrderNotFoundException()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the OrderNotFoundException class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public OrderNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the OrderNotFoundException class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public OrderNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the OrderNotFoundException class.
    /// </summary>
    /// <param name="orderId">The order ID that was not found.</param>
    public OrderNotFoundException(int orderId)
        : base($"Order with ID {orderId} was not found.")
    {
        this.OrderId = orderId;
    }

    /// <summary>
    /// Gets the order ID that was not found.
    /// </summary>
    public int OrderId { get; }
}

