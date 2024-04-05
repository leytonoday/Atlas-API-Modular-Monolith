using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Shared.Application.Abstractions.Messaging.Queue;

public abstract record QueuedCommand(Guid Id) : ICommand;
