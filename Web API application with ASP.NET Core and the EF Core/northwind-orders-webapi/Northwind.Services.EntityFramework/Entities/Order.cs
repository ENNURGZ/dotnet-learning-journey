using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.EntityFramework.Entities;

[Table("Orders")]
public class Order
{
    [Key]
    [Column("OrderID")]
    public long Id { get; set; }

    [Column("CustomerID")]
    public string CustomerId { get; set; } = default!;

    [Column("EmployeeID")]
    public long EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = default!;

    [Column("ShipVia")]
    public long ShipVia { get; set; }

    [ForeignKey(nameof(ShipVia))]
    public Shipper Shipper { get; set; } = default!;

    [Column("OrderDate")]
    public DateTime OrderDate { get; set; }

    [Column("RequiredDate")]
    public DateTime RequiredDate { get; set; }

    [Column("ShippedDate")]
    public DateTime? ShippedDate { get; set; }

    [Column("Freight")]
    public double Freight { get; set; }

    [Column("ShipName")]
    public string ShipName { get; set; } = default!;

    [Column("ShipAddress")]
    public string ShipAddress { get; set; } = default!;

    [Column("ShipCity")]
    public string ShipCity { get; set; } = default!;

    [Column("ShipRegion")]
    public string? ShipRegion { get; set; }

    [Column("ShipPostalCode")]
    public string ShipPostalCode { get; set; } = default!;

    [Column("ShipCountry")]
    public string ShipCountry { get; set; } = default!;

    public ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
