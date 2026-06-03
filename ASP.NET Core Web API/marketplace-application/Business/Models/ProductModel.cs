using System.Collections.Generic;

namespace Business.Models;

public class ProductModel
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public int ProductCategoryId { get; set; }

    public string? CategoryName { get; set; }

    public ICollection<int>? ReceiptDetailIds { get; set; }
}
