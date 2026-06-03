using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.EntityFramework.Entities;

[Table("Shippers")]
public class Shipper
{
    [Key]
    [Column("ShipperID")]
    public long Id { get; set; }

    [Column("CompanyName")]
    public string CompanyName { get; set; } = default!;
}
