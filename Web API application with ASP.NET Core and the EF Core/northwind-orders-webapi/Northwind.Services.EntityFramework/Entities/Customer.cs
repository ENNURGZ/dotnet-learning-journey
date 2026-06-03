using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.EntityFramework.Entities;

[Table("Customers")]
public class Customer
{
    [Key]
    [Column("CustomerID")]
    public string Id { get; set; } = default!;

    [Column("CompanyName")]
    public string CompanyName { get; set; } = default!;
}
