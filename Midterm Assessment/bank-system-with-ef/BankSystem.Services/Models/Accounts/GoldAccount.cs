using BankSystem.Services.Generators;

namespace BankSystem.Services.Models.Accounts;

/// <summary>
/// GoldAccount is a class representing a gold bank account in a banking system.
/// </summary>
public sealed class GoldAccount : BankAccount
{
    private const decimal GoldDepositCostPerPoint = 10m;
    private const decimal GoldWithdrawCostPerPoint = 5m;
    private const decimal GoldBalanceCostPerPoint = 5m;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoldAccount"/> class.
    /// </summary>
    public GoldAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator)
        : base(owner, currencyCode, uniqueNumberGenerator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GoldAccount"/> class.
    /// </summary>
    public GoldAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator)
        : base(owner, currencyCode, numberGenerator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GoldAccount"/> class.
    /// </summary>
    public GoldAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator, decimal initialBalance)
        : base(owner, currencyCode, uniqueNumberGenerator, initialBalance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GoldAccount"/> class.
    /// </summary>
    public GoldAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator, decimal initialBalance)
        : base(owner, currencyCode, numberGenerator, initialBalance)
    {
    }

    /// <summary>
    /// Gets the overdraft limit for the account.
    /// </summary>
    public override decimal Overdraft => 3 * this.BonusPoints;

    /// <summary>
    /// Calculates reward points based on balance and deposit amount.
    /// </summary>
    protected override int CalculateDepositRewardPoints(decimal amount)
    {
        return (int)Math.Max(Math.Ceiling(((this.Balance + amount) / GoldBalanceCostPerPoint) + (amount / GoldDepositCostPerPoint)), 0);
    }

    /// <summary>
    /// Calculates reward points based on balance and withdrawal amount.
    /// </summary>
    protected override int CalculateWithdrawRewardPoints(decimal amount)
    {
        return (int)Math.Max(Math.Ceiling(((this.Balance - amount) / GoldBalanceCostPerPoint) + (amount / GoldWithdrawCostPerPoint)), 0);
    }
}
