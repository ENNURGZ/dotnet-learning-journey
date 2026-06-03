using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Services.EntityFramework.Entities;

[Table("OrderDetails")]
[PrimaryKey(nameof(OrderId), nameof(ProductId))]
public class OrderDetail
{
    [Column("OrderID")]
    public long OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = default!;

    [Column("ProductID")]
    public long ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = default!;

    [Column("UnitPrice")]
    public double UnitPrice { get; set; }

    [Column("Quantity")]
    public long Quantity { get; set; }

    [Column("Discount")]
    public double Discount { get; set; }
}
