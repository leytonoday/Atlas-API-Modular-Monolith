namespace Atlas.Shared.Application.ModuleBridge;

/// <summary>
/// Defines methods for facilitating synchronous communication between modules.
/// </summary>
public interface IModuleBridge
{
    /// <summary>
    /// Retrieves the plan ID associated with the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>The plan ID associated with the user, or null if not found.</returns>
    public Task<Guid?> GetUserPlanId(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if the user has remaining credits.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>True if the user has remaining credits; otherwise, false.</returns>
    public Task<bool> DoesUserHaveCredits(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Decreases the credits of the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task DecreaseUserCredits(Guid userId, CancellationToken cancellationToken);
}
