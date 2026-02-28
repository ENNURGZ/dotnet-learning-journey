namespace MockTesting;

/// <summary>
/// Provides functionality for managing user accounts and balances.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Gets the current balance for a user's account.
    /// </summary>
    /// <param name="userId">The ID of the user whose balance to retrieve.</param>
    /// <returns>The current balance of the user's account.</returns>
    decimal GetBalance(int userId);
}
