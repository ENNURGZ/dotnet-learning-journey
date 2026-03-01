namespace MockTesting;

/// <summary>
/// Provides functionality for checking user permissions.
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Checks if a user has permission to perform transfers.
    /// </summary>
    /// <param name="userId">The ID of the user to check permissions for.</param>
    /// <returns>True if the user has transfer permission, false otherwise.</returns>
    bool HasTransferPermission(int userId);
}
