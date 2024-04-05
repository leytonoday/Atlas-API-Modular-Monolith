using Atlas.Shared.Application.Abstractions.Messaging.Queue;
using Atlas.Shared.Application.Queue;
using Autofac;
using Autofac.Features.Decorators;
using MediatR;

namespace Atlas.Shared.Infrastructure.Decorators;

class RequestIdempotenceDecorator<TRequest>(IRequestHandler<TRequest> decoratedHandler, IDecoratorContext context) : IIsDecorator, IRequestHandler<TRequest> where TRequest : class, IRequest
{
    private readonly IQueueWriter _queueWriter = context.Resolve<IQueueWriter>();

    public async Task Handle(TRequest request, CancellationToken cancellationToken)
    {
        string requestHandlerName = decoratedHandler.GetType().Name;

        if (request is QueuedCommand queuedCommand)
        {
            await HandleQueuedCommandIdempotence(requestHandlerName, queuedCommand, cancellationToken);
        }
        else
        {
            await decoratedHandler.Handle(request, cancellationToken);
        }
    }

    private async Task HandleQueuedCommandIdempotence(string requestHandlerName, QueuedCommand queuedCommand, CancellationToken cancellationToken) 
    {
        // Check if this command has been handled by this command handler previously
        bool alreadyHandled = await _queueWriter.IsQueueItemAlreadyHandledAsync(queuedCommand.Id, requestHandlerName, cancellationToken);

        if (alreadyHandled)
        {
            return;
        }

        // This is the actual command handler
        await decoratedHandler.Handle((queuedCommand as TRequest)!, cancellationToken);

        // If we get to this point, then we can assume an exception hasn't been thrown, and that the
        // command handler has executed successfully.

        // Create an entry to indicate that this command has been processed by this command handler
        _queueWriter.MarkQueueItemAsHandled(queuedCommand.Id, requestHandlerName, cancellationToken);
    }
}