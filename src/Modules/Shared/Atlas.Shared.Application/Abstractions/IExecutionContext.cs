namespace Atlas.Shared.Application.Abstractions;

/// <summary>
/// Provides access to execution context information such as user ID, correlation ID, availability, etc.
/// </summary>
public interface IExecutionContextAccessor
{
    /// <summary>
    /// Gets a value indicating whether the user is authenticated.
    /// </summary>
    public bool IsUserAuthenticated { get; }

    /// <summary>
    /// Gets the unique identifier (ID) of the user.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Indicates whether the execution context information is available.
    /// </summary>
    public bool IsAvailable { get; }
}