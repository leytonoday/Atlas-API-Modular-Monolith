using Atlas.Shared.Application.Queue;
using MediatR;

namespace Atlas.Shared.Application.Abstractions.Messaging.Queue;

public interface IQueuedCommandHandler<in TQueuedCommand> :
    IRequestHandler<TQueuedCommand>
    where TQueuedCommand : QueuedCommand;