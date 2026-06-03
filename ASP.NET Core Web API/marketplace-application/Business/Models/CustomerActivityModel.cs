namespace Business.Models;

public class CustomerActivityModel
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public decimal ReceiptSum { get; set; }
}
