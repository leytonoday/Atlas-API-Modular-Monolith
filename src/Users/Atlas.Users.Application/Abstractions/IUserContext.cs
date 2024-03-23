namespace Atlas.Users.Application.Abstractions;

/// <summary>
/// Represents the context of the current user, providing arbitrary information about the user.
/// </summary>
public interface IUserContext
{
    /// <summary>
    /// Gets a value indicating whether the user is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Gets the unique identifier (ID) of the user.
    /// </summary>
    Guid UserId { get; }
}