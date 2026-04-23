using BankSystem.Services.Generators;

namespace BankSystem.Services.Models.Accounts;

/// <summary>
/// SilverAccount is a class representing a silver bank account in a banking system.
/// </summary>
public sealed class SilverAccount : BankAccount
{
    private const decimal SilverDepositCostPerPoint = 5m;
    private const decimal SilverWithdrawCostPerPoint = 2m;
    private const decimal SilverBalanceCostPerPoint = 100m;

    /// <summary>
    /// Initializes a new instance of the <see cref="SilverAccount"/> class.
    /// </summary>
    public SilverAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator)
        : base(owner, currencyCode, uniqueNumberGenerator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SilverAccount"/> class.
    /// </summary>
    public SilverAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator)
        : base(owner, currencyCode, numberGenerator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SilverAccount"/> class.
    /// </summary>
    public SilverAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator, decimal initialBalance)
        : base(owner, currencyCode, uniqueNumberGenerator, initialBalance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SilverAccount"/> class.
    /// </summary>
    public SilverAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator, decimal initialBalance)
        : base(owner, currencyCode, numberGenerator, initialBalance)
    {
    }

    /// <summary>
    /// Gets the overdraft limit for the account.
    /// </summary>
    public override decimal Overdraft => 2 * this.BonusPoints;

    /// <summary>
    /// Calculates reward points based on balance and deposit amount.
    /// </summary>
    protected override int CalculateDepositRewardPoints(decimal amount)
    {
        return (int)Math.Max(Math.Floor(((this.Balance + amount) / SilverBalanceCostPerPoint) + (amount / SilverDepositCostPerPoint)), 0);
    }

    /// <summary>
    /// Calculates reward points based on balance and withdrawal amount.
    /// </summary>
    protected override int CalculateWithdrawRewardPoints(decimal amount)
    {
        return (int)Math.Max(Math.Floor(((this.Balance - amount) / SilverBalanceCostPerPoint) + (amount / SilverWithdrawCostPerPoint)), 0);
    }
}
