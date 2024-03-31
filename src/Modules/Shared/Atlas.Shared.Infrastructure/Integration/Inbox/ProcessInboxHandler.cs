using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Commands;
using Atlas.Shared.Infrastructure.Queue;
using Atlas.Shared.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Atlas.Shared.Infrastructure.Integration.Inbox;

public class ProcessInboxHandler(InboxReader inboxReader, IPublisher publisher, ILogger<ProcessInboxHandler> logger) : ICommandHandler<ProcessInboxCommand>
{
    public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        List<InboxMessage> messages = await inboxReader.ListPendingAsync(cancellationToken);

        logger.LogDebug("Found {messagesCount} pending messages in inbox.", messages.Count);

        foreach (var message in messages)
        {
            logger.LogInformation($"Processing outbox message: {message.Id} {message.Type} {message.Data}");
            IIntegrationEvent? integrationEvent = InboxMessage.ToIntegrationEvent(message);
            if (integrationEvent is null) 
            {
                continue;
            }

            await publisher.Publish(integrationEvent, cancellationToken);

            message.MarkProcessed();
            await inboxReader.MarkProcessedAsync(message, cancellationToken);
        }
    }
}