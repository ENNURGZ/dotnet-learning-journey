using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystem.EF.Entities;

[Table("account_owner")]
public class AccountOwner
{
    [Key]
    [Column("account_owner_id")]
    public int Id { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; }

    [Column("last_name")]
    public string LastName { get; set; }

    [Column("email")]
    public string Email { get; set; }

    public virtual ICollection<BankAccount> BankAccounts { get; set; }
}
