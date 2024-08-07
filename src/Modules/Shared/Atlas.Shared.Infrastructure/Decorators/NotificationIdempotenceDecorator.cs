﻿using Atlas.Shared.Application.Abstractions.Integration.Inbox;
using Atlas.Shared.Domain;
using Atlas.Shared.IntegrationEvents;
using MediatR;

namespace Atlas.Shared.Infrastructure.Decorators;

/// <summary>
/// Decorator for notification handlers that ensures that notifications are not handled more than once.
/// </summary>
/// <typeparam name="TNotification">The type of notification being handled.</typeparam>
class NotificationIdempotenceDecorator<TNotification>(INotificationHandler<TNotification> decoratedHandler, IInboxWriter inboxWriter) : IIsDecorator, INotificationHandler<TNotification> where TNotification : class, INotification
{
    /// <summary>
    /// Asynchronously handles a notification request.
    /// </summary>
    /// <param name="request">The notification to be processed.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Handle(TNotification request, CancellationToken cancellationToken)
    {
        // Decorators will decorate OTHER decorators if they implemenet the same interface (INotificationHandler in this case). In this cases, this is desireable. For example,
        // The NotificationUnitOfWorkDecorator should decorate this one and commit it's changes as a unit of work, but I don't want THIS handler decorating that one.
        if (IIsDecorator.IsDecorator(decoratedHandler.GetType()))
        {
            return;
        }

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

    /// <summary>
    /// Handles idempotence for integration events by checking if the event has already been processed by this handler.
    /// </summary>
    /// <param name="requestHandlerName">The name of the request handler (decorated handler).</param>
    /// <param name="integrationEvent">The integration event to be processed.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task HandleIntegrationEventIdempotence(string requestHandlerName, IntegrationEvent integrationEvent, CancellationToken cancellationToken) 
    {
        // Check if this inbox message has been handled by this handler previously
        bool alreadyHandled = await inboxWriter.IsInboxItemAlreadyHandledAsync(integrationEvent.Id, requestHandlerName, cancellationToken);

        if (alreadyHandled)
        {
            return;
        }

        // This is the actual command handler
        await decoratedHandler.Handle((integrationEvent as TNotification)!, cancellationToken);

        // If we get to this point, then we can assume an exception hasn't been thrown, and that the
        // integration event handler has executed successfully.

         // Create an entry to indicate that this inbox message has been processed by this integration event handler
        inboxWriter.MarkInboxItemAsHandled(integrationEvent.Id, requestHandlerName, cancellationToken);
    }


}