using BankSystem.Services.Generators;

namespace BankSystem.Services.Models.Accounts;

/// <summary>
/// StandardAccount is a class representing a standard bank account in a banking system.
/// </summary>
public sealed class StandardAccount : BankAccount
{
    private const decimal StandardBalanceCostPerPoint = 100m;

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardAccount"/> class.
    /// </summary>
    public StandardAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator)
        : base(owner, currencyCode, uniqueNumberGenerator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardAccount"/> class.
    /// </summary>
    public StandardAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator)
        : base(owner, currencyCode, numberGenerator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardAccount"/> class.
    /// </summary>
    public StandardAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator, decimal initialBalance)
        : base(owner, currencyCode, uniqueNumberGenerator, initialBalance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardAccount"/> class.
    /// </summary>
    public StandardAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator, decimal initialBalance)
        : base(owner, currencyCode, numberGenerator, initialBalance)
    {
    }

    /// <summary>
    /// Gets the overdraft limit for the account.
    /// </summary>
    public override decimal Overdraft => 0;

    /// <summary>
    /// Calculates reward points based on balance only.
    /// </summary>
    protected override int CalculateDepositRewardPoints(decimal amount)
    {
        return (int)Math.Max(Math.Floor((this.Balance + amount) / StandardBalanceCostPerPoint), 0);
    }

    /// <summary>
    /// Calculates reward points based on balance only.
    /// </summary>
    protected override int CalculateWithdrawRewardPoints(decimal amount)
    {
        return (int)Math.Max(Math.Floor((this.Balance - amount) / StandardBalanceCostPerPoint), 0);
    }
}
