using BankSystem.Services.Generators;
using BankSystem.Services.Models;
using BankSystem.Services.Models.Accounts;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace BankSystem.Tests.Models;

[TestFixture]
public class BankAccountTests
{
    private Mock<BankAccount> mockBankAccount = null!;
    private Mock<IUniqueNumberGenerator> mockNumberGenerator = null!;

    [SetUp]
    public void SetUp()
    {
        var owner = new AccountOwner("John", "Doe", "john.doe@gmail.com");
        this.mockNumberGenerator = new Mock<IUniqueNumberGenerator>();
        _ = this.mockNumberGenerator.Setup(m => m.Generate()).Returns("1234567890");
        this.mockBankAccount = new Mock<BankAccount>(owner, "USD", this.mockNumberGenerator.Object, 1000m);
    }

    [Test]
    public void Deposit_IncreaseBalance()
    {
        this.mockBankAccount.Object.Deposit(500m, DateTime.Now, "Test deposit");
        Assert.That(1500, Is.EqualTo(this.mockBankAccount.Object.Balance));
        this.mockBankAccount.Protected().Verify("CalculateDepositRewardPoints", Times.Exactly(2), ItExpr.IsAny<decimal>());
    }

    [Test]
    public void Withdraw_DecreaseBalance()
    {
        this.mockBankAccount.Object.Withdraw(500m, DateTime.Now, "Test withdrawal");
        Assert.That(500, Is.EqualTo(this.mockBankAccount.Object.Balance));
    }

    [Test]
    public void Withdraw_InvokeCalculateWithdrawRewardPointsMethod()
    {
        this.mockBankAccount.Object.Withdraw(500m, DateTime.Now, "Test withdrawal");
        Assert.That(500, Is.EqualTo(this.mockBankAccount.Object.Balance));
        this.mockBankAccount.Protected().Verify("CalculateWithdrawRewardPoints", Times.Once(), ItExpr.IsAny<decimal>());
    }

    [Test]
    public void Withdraw_AmountIsGreaterThanBalance_ThrowInvalidOperationException()
    {
        _ = Assert.Throws<InvalidOperationException>(() => this.mockBankAccount.Object.Withdraw(2000, DateTime.Now, "Test excessive withdrawal"));
    }

    [Test]
    public void Constructor_AccountNumberGeneratedByNumberGenerator()
    {
        Assert.That("1234567890", Is.EqualTo(this.mockBankAccount.Object.Number));
        this.mockNumberGenerator.Verify(m => m.Generate(), Times.Once);
    }

    [Test]
    public void ToString_ReturnInformationAboutBankAccount()
    {
        string s = @"John Doe, john.doe@gmail.com. No:1234567891. Balance: 1000USD.\\n\";
        _ = this.mockBankAccount.Setup(b => b.ToString()).Returns(s);
        Assert.That(s, Is.EqualTo(this.mockBankAccount.Object.ToString()));
    }
}
