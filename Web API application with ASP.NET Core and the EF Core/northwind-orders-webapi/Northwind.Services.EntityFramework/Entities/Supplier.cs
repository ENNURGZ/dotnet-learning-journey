using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.EntityFramework.Entities;

[Table("Suppliers")]
public class Supplier
{
    [Key]
    [Column("SupplierID")]
    public long Id { get; set; }

    [Column("CompanyName")]
    public string CompanyName { get; set; } = default!;
}
