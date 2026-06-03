using System.Collections.Generic;

namespace Data.Entities;

public class ProductCategory : BaseEntity
{
    public string CategoryName { get; set; } = null!;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}