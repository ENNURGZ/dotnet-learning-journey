using BankSystem.EF.Entities;
using BankSystem.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Services.Services;

public sealed class AccountService : BaseService
{
    public AccountService(BankContext context)
        : base(context)
    {
    }

    public IReadOnlyList<BankAccountFullInfoModel> GetBankAccountsFullInfo()
    {
        return this.Context.BankAccounts
            .OrderBy(b => b.Id)
            .Select(b => new BankAccountFullInfoModel
            {
                BankAccountId = b.Id,
                FirstName = b.AccountOwner.FirstName,
                LastName = b.AccountOwner.LastName,
                AccountNumber = b.Number,
                Balance = b.Balance,
                CurrencyCode = b.CurrencyCode.CurrenciesCode,
                BonusPoints = b.BonusPoints
            })
            .ToList();
    }
}
