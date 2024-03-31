using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Commands;
using Atlas.Shared.Infrastructure.Integration.Bus;
using Atlas.Shared.Infrastructure.Queue;
using Atlas.Shared.IntegrationEvents;
using Microsoft.Extensions.Logging;

namespace Atlas.Shared.Infrastructure.Integration.Outbox;

public class ProcessOutboxHandler(OutboxReader outboxReader, IEventBus eventBus, ILogger<ProcessOutboxHandler> logger) : ICommandHandler<ProcessOutboxCommand>
{
    public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        List<OutboxMessage> messages = await outboxReader.ListPendingAsync(cancellationToken);

        logger.LogDebug("Found {messagesCount} pending messages in outbox.", messages.Count);

        foreach (var message in messages)
        {
            logger.LogInformation($"Processing outbox message: {message.Id} {message.Type} {message.Data}");
            IIntegrationEvent? integrationEvent = OutboxMessage.ToIntegrationEvent(message);
            if (integrationEvent is null) 
            {
                continue;
            }

            await eventBus.Publish(integrationEvent, cancellationToken);

            message.MarkProcessed();
            await outboxReader.MarkProcessedAsync(message, cancellationToken);
        }
    }
}