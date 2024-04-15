using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Shared.Application.Abstractions.Messaging.Queue;

/// <summary>
/// Represents a command that can be stored within the module's Queue.
/// </summary>
public abstract record QueuedCommand : ICommand
{
    /// <summary>
    /// Gets the unique identifier for the queued command.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();
}
