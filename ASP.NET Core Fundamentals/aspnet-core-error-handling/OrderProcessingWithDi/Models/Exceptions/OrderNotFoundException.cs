namespace OrderProcessingWithDi.Models.Exceptions;

/// <summary>
/// Exception thrown when an order is not found.
/// TODO: Implement this exception class.
/// 
/// Requirements:
/// - Inherit from Exception
/// - Add property OrderId (int) to store the order ID that was not found
/// - Add constructor: OrderNotFoundException(int orderId)
///   - Should set message to: $"Order with ID {orderId} was not found."
///   - Should store orderId in OrderId property
/// - Include standard Exception constructors for CA1032 compliance:
///   - OrderNotFoundException()
///   - OrderNotFoundException(string message)
///   - OrderNotFoundException(string message, Exception innerException)
/// </summary>
public class OrderNotFoundException : Exception
{
    public int OrderId { get; }

    public OrderNotFoundException()
        : base()
    {
    }

    public OrderNotFoundException(string message)
        : base(message)
    {
    }

    public OrderNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public OrderNotFoundException(int orderId)
        : base($"Order with ID {orderId} was not found.")
    {
        this.OrderId = orderId;
    }
}

