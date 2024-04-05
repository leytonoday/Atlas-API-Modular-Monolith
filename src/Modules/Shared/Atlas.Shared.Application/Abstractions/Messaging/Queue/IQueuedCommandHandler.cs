using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Shared.Application.Abstractions.Messaging.Queue;

public interface IQueuedCommandHandler<in TQueuedCommand> : IRequestHandler<TQueuedCommand>
    where TQueuedCommand : ICommand;