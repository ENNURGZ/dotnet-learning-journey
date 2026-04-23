namespace BankSystem.Services.Models;

public class AccountOwnerTotalBalanceModel
{
    public int AccountOwnerId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string CurrencyCode { get; set; }

    public decimal Total { get; set; }
}
