namespace MockTesting;

/// <summary>
/// Provides functionality for processing financial transactions.
/// </summary>
public interface ITransactionService
{
    /// <summary>
    /// Transfers funds from one user's account to another.
    /// </summary>
    /// <param name="fromUserId">The ID of the user sending the funds.</param>
    /// <param name="toUserId">The ID of the user receiving the funds.</param>
    /// <param name="amount">The amount of funds to transfer.</param>
    /// <exception cref="InvalidOperationException">Thrown when the transfer cannot be completed.</exception>
    void Transfer(int fromUserId, int toUserId, decimal amount);
}
