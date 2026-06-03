using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.EntityFramework.Entities;

[Table("Products")]
public class Product
{
    [Key]
    [Column("ProductID")]
    public long Id { get; set; }

    [Column("ProductName")]
    public string ProductName { get; set; } = default!;

    [Column("SupplierID")]
    public long SupplierId { get; set; }

    [ForeignKey(nameof(SupplierId))]
    public Supplier Supplier { get; set; } = default!;

    [Column("CategoryID")]
    public long CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = default!;
}
