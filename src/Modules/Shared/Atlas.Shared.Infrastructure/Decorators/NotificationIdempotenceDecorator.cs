using Atlas.Shared.Application.Abstractions.Integration.Inbox;
using Atlas.Shared.Application.Abstractions.Messaging.Queue;
using Atlas.Shared.Application.Queue;
using Atlas.Shared.IntegrationEvents;
using Autofac;
using Autofac.Features.Decorators;
using MediatR;

namespace Atlas.Shared.Infrastructure.Decorators;

class NotificationIdempotenceDecorator<TNotification>(INotificationHandler<TNotification> decoratedHandler, IDecoratorContext context) : INotificationHandler<TNotification> where TNotification : class, INotification
{
    private readonly IInboxWriter _inboxWriter = context.Resolve<IInboxWriter>();

    public async Task Handle(TNotification request, CancellationToken cancellationToken)
    {
        string requestHandlerName = decoratedHandler.GetType().Name;

        if (request is IntegrationEvent integrationEvent)
        {
            await HandleIntegrationEventIdempotence(requestHandlerName, integrationEvent, cancellationToken);
        }
        else
        {
            await decoratedHandler.Handle(request, cancellationToken);
        }
    }

    private async Task HandleIntegrationEventIdempotence(string requestHandlerName, IntegrationEvent integrationEvent, CancellationToken cancellationToken) 
    {
        // Check if this inbox message has been handled by this handler previously
        bool alreadyHandled = await _inboxWriter.IsInboxItemAlreadyHandledAsync(integrationEvent.Id, requestHandlerName, cancellationToken);

        if (alreadyHandled)
        {
            return;
        }

        // This is the actual command handler
        await decoratedHandler.Handle((integrationEvent as TNotification)!, cancellationToken);

        // If we get to this point, then we can assume an exception hasn't been thrown, and that the
        // integration event handler has executed successfully.

        // Create an entry to indicate that this inbox message has been processed by this integration event handler
        _inboxWriter.MarkInboxItemAsHandled(integrationEvent.Id, requestHandlerName, cancellationToken);
    }
}