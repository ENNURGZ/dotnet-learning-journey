using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.EntityFramework.Entities;

[Table("Employees")]
public class Employee
{
    [Key]
    [Column("EmployeeID")]
    public long Id { get; set; }

    [Column("FirstName")]
    public string FirstName { get; set; } = default!;

    [Column("LastName")]
    public string LastName { get; set; } = default!;

    [Column("Country")]
    public string Country { get; set; } = default!;
}
