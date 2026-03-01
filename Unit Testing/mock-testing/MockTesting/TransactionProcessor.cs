using System.Diagnostics.CodeAnalysis;

namespace MockTesting;

/// <summary>
/// Processes financial transactions between user accounts.
/// </summary>
public class TransactionProcessor
{
    private readonly IPermissionService permissionService;
    private readonly IAccountService accountService;
    private readonly ITransactionService transactionService;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionProcessor"/> class.
    /// </summary>
    /// <param name="permissionService">The service for checking user permissions.</param>
    /// <param name="accountService">The service for managing user accounts.</param>
    /// <param name="transactionService">The service for processing transactions.</param>
    /// <param name="logger">The service for logging messages.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the dependencies are null.</exception>
    public TransactionProcessor(
        IPermissionService permissionService,
        IAccountService accountService,
        ITransactionService transactionService,
        ILogger logger)
    {
        this.permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
        this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        this.transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Processes a transfer between two users.
    /// </summary>
    /// <param name="fromUserId">Source user ID.</param>
    /// <param name="toUserId">Destination user ID.</param>
    /// <param name="amount">Amount to transfer.</param>
    /// <returns>True if transfer was successful, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown when input parameters are invalid.</exception>
    public bool ProcessTransfer(int fromUserId, int toUserId, decimal amount)
    {
        ValidateInput(fromUserId, toUserId, amount);

        var transactionId = Guid.NewGuid();
        this.LogTransactionStart(transactionId, fromUserId, toUserId, amount);

        if (!this.permissionService.HasTransferPermission(fromUserId))
        {
            this.LogTransactionFailure(transactionId, "Permission denied");
            return false;
        }

        if (this.accountService.GetBalance(fromUserId) < amount)
        {
            this.LogTransactionFailure(transactionId, $"Insufficient funds. Current balance: {this.accountService.GetBalance(fromUserId)}");
            return false;
        }

        try
        {
            this.transactionService.Transfer(fromUserId, toUserId, amount);
            this.LogTransactionSuccess(transactionId);
            return true;
        }
        catch (Exception ex)
        {
            this.LogTransactionError(transactionId, ex);
            return false;
        }
    }

    /// <summary>
    /// Validates the input parameters for a transfer.
    /// </summary>
    /// <param name="fromUserId">Source user ID.</param>
    /// <param name="toUserId">Destination user ID.</param>
    /// <param name="amount">Amount to transfer.</param>
    /// <exception cref="ArgumentException">Thrown when any parameter is invalid.</exception>
    [MemberNotNull(nameof(permissionService), nameof(accountService), nameof(transactionService), nameof(logger))]
    private static void ValidateInput(int fromUserId, int toUserId, decimal amount)
    {
        if (fromUserId <= 0)
        {
            throw new ArgumentException("Source user ID must be positive", nameof(fromUserId));
        }

        if (toUserId <= 0)
        {
            throw new ArgumentException("Destination user ID must be positive", nameof(toUserId));
        }

        if (fromUserId == toUserId)
        {
            throw new ArgumentException("Source and destination users cannot be the same");
        }

        if (amount <= 0)
        {
            throw new ArgumentException("Transfer amount must be positive", nameof(amount));
        }
    }

    /// <summary>
    /// Logs the start of a transaction.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction.</param>
    /// <param name="fromUserId">Source user ID.</param>
    /// <param name="toUserId">Destination user ID.</param>
    /// <param name="amount">Amount to transfer.</param>
    private void LogTransactionStart(Guid transactionId, int fromUserId, int toUserId, decimal amount)
    {
        this.logger.Log($"Transaction {transactionId} started: Transfer {amount} from {fromUserId} to {toUserId}");
    }

    /// <summary>
    /// Logs the successful completion of a transaction.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction.</param>
    private void LogTransactionSuccess(Guid transactionId)
    {
        this.logger.Log($"Transaction {transactionId} completed successfully");
    }

    /// <summary>
    /// Logs a transaction failure.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction.</param>
    /// <param name="reason">The reason for the failure.</param>
    private void LogTransactionFailure(Guid transactionId, string reason)
    {
        this.logger.Log($"Transaction {transactionId} failed: {reason}");
    }

    /// <summary>
    /// Logs a transaction error.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction.</param>
    /// <param name="ex">The exception that caused the error.</param>
    private void LogTransactionError(Guid transactionId, Exception ex)
    {
        this.logger.Log($"Transaction {transactionId} failed with error: {ex.Message}");
    }
}
