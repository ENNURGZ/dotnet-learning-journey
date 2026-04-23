using BankSystem.EF.Entities;
using BankSystem.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Services.Services;

public sealed class OwnerService : BaseService
{
    public OwnerService(BankContext context)
        : base(context)
    {
    }

    public IReadOnlyList<AccountOwnerTotalBalanceModel> GetAccountOwnersTotalBalance()
    {
        return this.Context.BankAccounts
            .GroupBy(b => new
            {
                b.AccountOwnerId,
                b.AccountOwner.FirstName,
                b.AccountOwner.LastName,
                b.CurrencyCode.CurrenciesCode
            })
            .AsEnumerable()
            .Select(g => new AccountOwnerTotalBalanceModel
            {
                AccountOwnerId = g.Key.AccountOwnerId,
                FirstName = g.Key.FirstName,
                LastName = g.Key.LastName,
                CurrencyCode = g.Key.CurrenciesCode,
                Total = (decimal)g.Sum(b => (double)b.Balance)
            })
            .OrderByDescending(m => m.Total)
            .ToList();
    }
}
