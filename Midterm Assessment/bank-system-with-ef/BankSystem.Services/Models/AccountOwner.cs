using System.Collections.ObjectModel;
using BankSystem.Services.Helpers;
using BankSystem.Services.Models.Accounts;

namespace BankSystem.Services.Models;

/// <summary>
/// The AccountOwner class is used for representing a bank account owner.
/// </summary>
public sealed class AccountOwner
{
    private readonly List<BankAccount> accounts = new List<BankAccount>();

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountOwner"/> class.
    /// </summary>
    /// <param name="firstName">The first name of the account owner.</param>
    /// <param name="lastName">The last name of the account owner.</param>
    /// <param name="email">The email address of the account owner.</param>
    /// <exception cref="ArgumentException">Thrown when first name, last name is empty or email is invalid.</exception>
    public AccountOwner(string firstName, string lastName, string email)
    {
        VerifyString(firstName, nameof(firstName));
        VerifyString(lastName, nameof(lastName));

        if (!email.IsEmailValid())
        {
            throw new ArgumentException("Invalid email format.", nameof(email));
        }

        this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
    }

    /// <summary>
    /// Gets the account owner's first name.
    /// </summary>
    public string FirstName { get; }

    /// <summary>
    /// Gets the account owner's last name.
    /// </summary>
    public string LastName { get; }

    /// <summary>
    /// Gets the account owner's email address.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Gets a list of bank accounts associated with the account owner.
    /// </summary>
    public List<BankAccount> OwnerAccounts => this.accounts;

    /// <summary>
    /// ToString() method returns a string containing the name and email of the account owner.
    /// </summary>
    /// <returns>A string containing the name and email of the account owner.</returns>
    public override string ToString()
    {
        return $"{this.FirstName} {this.LastName}, {this.Email}.";
    }

    /// <summary>
    /// Add(BankAccount account) method allows adding a new BankAccount instance to the list of accounts owned by the account owner.
    /// </summary>
    /// <param name="account">The bank account to add.</param>
    public void Add(BankAccount account)
    {
        this.accounts.Add(account);
    }

    /// <summary>
    /// Accounts() method returns a list of BankAccount instances associated with the bank account owner.
    /// </summary>
    /// <returns>A list of bank accounts.</returns>
    public ReadOnlyCollection<BankAccount> Accounts()
    {
        return this.accounts.AsReadOnly();
    }

    private static void VerifyString(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", paramName);
        }
    }
}
