using NUnit.Framework;

namespace BankSystem.Tests.Models;

[TestFixture]
public sealed class AccountCashOperationTests
{
    private static readonly object[] Operation =
    [
        new object[] { 1000m, new DateTime(2024, 1, 23, 18, 30, 25), "Deposit test", "01/23/2024 18:30:25 Deposit test : Credited to account 1000." },
        new object[] { -500m, new DateTime(2024, 2, 15, 12, 3, 34), "Withdrawal test", "02/15/2024 12:03:34 Withdrawal test : Debited from account -500." },
        new object[] { 0m, new DateTime(2024, 03, 1, 4, 23, 45), "Balance test", "03/01/2024 04:23:45 Balance test : Credited to account 0." },
        new object[] { 20000m, new DateTime(2024, 4, 1, 22, 3, 4), "New Year Deposit", "04/01/2024 22:03:04 New Year Deposit : Credited to account 20000." },
        new object[] { -3250m, new DateTime(2024, 5, 26, 3, 27, 54), "Vacation Withdrawal", "05/26/2024 03:27:54 Vacation Withdrawal : Debited from account -3250." },
        new object[] { 15000m, new DateTime(2024, 6, 12, 11, 46, 23), "Salary Deposit", "06/12/2024 11:46:23 Salary Deposit : Credited to account 15000." },
        new object[] { -1000m, new DateTime(2024, 7, 20, 14, 5, 33), "Groceries Withdrawal", "07/20/2024 14:05:33 Groceries Withdrawal : Debited from account -1000." },
    ];

    [TestCaseSource(typeof(AccountCashOperationTests), nameof(Operation))]
    public void Operation_ObjectVerification(decimal amount, DateTime date, string note, string expected)
    {
        var operation = new BankSystem.Services.Models.AccountCashOperation(amount, date, note);

        Assert.That(operation.Amount, Is.EqualTo(amount));
        Assert.That(operation.Date, Is.EqualTo(date));
        Assert.That(operation.Note, Is.EqualTo(note));
        Assert.That(operation.ToString(), Is.EqualTo(expected));
    }
}
