using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Shared.Application.Abstractions.Messaging.Queue;

public abstract record QueuedCommand : ICommand
{
    public Guid Id { get; } = Guid.NewGuid();
}
