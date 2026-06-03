using System.Collections.Generic;

namespace Data.Entities;

public class Product : BaseEntity
{
    public int ProductCategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public ProductCategory Category { get; set; } = null!;

    public ICollection<ReceiptDetail>? ReceiptDetails { get; set; }
}