using System.Collections.ObjectModel;
using BankSystem.Services.Generators;
using BankSystem.Services.Helpers;

namespace BankSystem.Services.Models.Accounts;

/// <summary>
/// The BankAccount class provides a way to work with bank accounts. This is an abstract class, so it cannot be instantiated directly.
/// </summary>
public abstract class BankAccount
{
    private readonly List<AccountCashOperation> operations = new List<AccountCashOperation>();

    /// <summary>
    /// Initializes a new instance of the <see cref="BankAccount"/> class.
    /// </summary>
    protected BankAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator)
        : this(owner, currencyCode, uniqueNumberGenerator.Generate, 0m)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankAccount"/> class.
    /// </summary>
    protected BankAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator)
        : this(owner, currencyCode, numberGenerator, 0m)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankAccount"/> class.
    /// </summary>
    protected BankAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator, decimal initialBalance)
        : this(owner, currencyCode, uniqueNumberGenerator.Generate, initialBalance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BankAccount"/> class.
    /// </summary>
    protected BankAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator, decimal initialBalance)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(currencyCode);
        ArgumentNullException.ThrowIfNull(numberGenerator);

        if (!currencyCode.IsCurrencyValid())
        {
            throw new ArgumentException("Invalid currency code.", nameof(currencyCode));
        }

        this.AccountOwner = owner;
        this.CurrencyCode = currencyCode;
        this.Number = numberGenerator();
        this.Balance = 0;
        this.BonusPoints = 0;

        if (initialBalance > 0)
        {
            this.Deposit(initialBalance, DateTime.Now, "Initial deposit");
        }
    }

    /// <summary>
    /// Gets the bank account number.
    /// </summary>
    public string Number { get; }

    /// <summary>
    /// Gets the balance of the account.
    /// </summary>
    public decimal Balance { get; protected set; }

    /// <summary>
    /// Gets the ISO currency code for the account.
    /// </summary>
    public string CurrencyCode { get; }

    /// <summary>
    /// Gets the owner of the bank account.
    /// </summary>
    public AccountOwner AccountOwner { get; }

    /// <summary>
    /// Gets or sets the bonus points associated with the account.
    /// </summary>
    public int BonusPoints { get; protected set; }

    /// <summary>
    /// Gets the overdraft limit for the account.
    /// </summary>
    public abstract decimal Overdraft { get; }

    /// <summary>
    /// GetAllOperations() method returns all operations performed on the account.
    /// </summary>
    /// <returns>A list of operations.</returns>
    public ReadOnlyCollection<AccountCashOperation> GetAllOperations()
    {
        return this.operations.AsReadOnly();
    }

    /// <summary>
    /// Deposit() method allows depositing money to account.
    /// </summary>
    /// <param name="amount">The amount to deposit.</param>
    /// <param name="date">The date of the operation.</param>
    /// <param name="note">The note for the operation.</param>
    public void Deposit(decimal amount, DateTime date, string note)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount must be non-negative.", nameof(amount));
        }

        this.BonusPoints += this.CalculateDepositRewardPoints(amount);
        this.Balance += amount;
        this.operations.Add(new AccountCashOperation(amount, date, note));
    }

    /// <summary>
    /// Withdraw() method allows withdrawing money from the account.
    /// </summary>
    /// <param name="amount">The amount to withdraw.</param>
    /// <param name="date">The date of the operation.</param>
    /// <param name="note">The note for the operation.</param>
    public void Withdraw(decimal amount, DateTime date, string note)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount must be non-negative.", nameof(amount));
        }

        if (this.Balance + this.Overdraft < amount)
        {
            throw new InvalidOperationException("Insufficient funds including overdraft.");
        }

        this.BonusPoints += this.CalculateWithdrawRewardPoints(amount);
        this.Balance -= amount;
        this.operations.Add(new AccountCashOperation(-amount, date, note));
    }

    /// <summary>
    /// Calculates reward points upon deposit.
    /// </summary>
    /// <param name="amount">The deposit amount.</param>
    /// <returns>The reward points.</returns>
    protected abstract int CalculateDepositRewardPoints(decimal amount);

    /// <summary>
    /// Calculates reward points upon withdrawal.
    /// </summary>
    /// <param name="amount">The withdrawal amount.</param>
    /// <returns>The reward points.</returns>
    protected abstract int CalculateWithdrawRewardPoints(decimal amount);

    /// <summary>
    /// Returns a string representation of the bank account.
    /// </summary>
    /// <returns>A string representation of the bank account.</returns>
    public override string ToString()
    {
        return $"{this.AccountOwner.ToString()} No:{this.Number}. Balance: {this.Balance}{this.CurrencyCode}.";
    }
}
