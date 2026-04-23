using System.Data.Common;
using BankSystem.Services.Models;

namespace BankSystem.Tests.Services;

public static class ModelBuilders
{
    public static AccountOwnerTotalBalanceModel BuildAcountOwnerTotalBalanceModel(DbDataReader reader)
    {
        return new AccountOwnerTotalBalanceModel
        {
            AccountOwnerId = (int)reader.GetInt64(0),
            FirstName = reader.GetString(1),
            LastName = reader.GetString(2),
            CurrencyCode = reader.GetString(3),
            Total = reader.GetDecimal(4),
        };
    }

    public static BankAccountFullInfoModel BuildBankAccountFullInfoModel(DbDataReader reader)
    {
        return new BankAccountFullInfoModel
        {
            BankAccountId = (int)reader.GetInt64(0),
            FirstName = reader.GetString(1),
            LastName = reader.GetString(2),
            AccountNumber = reader.GetString(3),
            Balance = reader.GetDecimal(4),
            CurrencyCode = reader.GetString(5),
            BonusPoints = (int)reader.GetInt64(6),
        };
    }
}
