namespace OrderProcessingWithDi.Models;

/// <summary>
/// Represents the result of processing an order.
/// </summary>
public record OrderResult
{
    public OrderResult() { }

    public OrderResult(string productId, int quantity, decimal total)
    {
        ProductId = productId;
        Quantity = quantity;
        Total = total;
        ProcessedAt = DateTime.UtcNow;
    }

    public string ProductId { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal Total { get; init; }
    public DateTime ProcessedAt { get; init; } = DateTime.UtcNow;
}