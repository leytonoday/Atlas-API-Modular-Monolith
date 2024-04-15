using MediatR;

namespace Atlas.Shared.Application.Abstractions.Messaging.Queue;

/// <summary>
/// Defines a contract for handlers that can process queued commands.
/// </summary>
/// <typeparam name="TQueuedCommand">The type of the queued command this handler can process. 
/// Must inherit from <see cref="QueuedCommand"/>.</typeparam>
public interface IQueuedCommandHandler<in TQueuedCommand> : IRequestHandler<TQueuedCommand>
    where TQueuedCommand : QueuedCommand;