using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Commands;
using Atlas.Shared.Infrastructure.Integration.Bus;
using Atlas.Shared.Infrastructure.Integration.Outbox;
using Atlas.Shared.IntegrationEvents;
using Microsoft.Extensions.Logging;
using Polly.Retry;
using Polly;
using Atlas.Shared.Application.Abstractions.Services;

namespace Atlas.Shared.Infrastructure.Integration.Handlers;

public class ProcessOutboxCommandHandler(IOutboxReader outboxReader, IEventBus eventBus, ILogger<ProcessOutboxCommandHandler> logger, ISupportNotifierService supportNotifierService) : ICommandHandler<ProcessOutboxCommand>
{
    /// <summary>
    /// A retry policy from the Polly library that will attempt to execute something and re-try 3 times if it 
    /// fails, with the time in-between each re-try increasing by 100 milliseconds. Basically a back-off algorithm
    /// </summary>
    private readonly static AsyncRetryPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(
            3,
            attempt => TimeSpan.FromMilliseconds(100 * attempt)
    );

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

            PolicyResult result = await _retryPolicy.ExecuteAndCaptureAsync(() => eventBus.Publish(integrationEvent, cancellationToken));

            if (result.Outcome == OutcomeType.Failure)
            {
                // Log the final exception and mark the queue message with the exception details
                logger.LogError(result.FinalException, "Cannot publish the OutboxMessage with Id {messageId}", message.Id);
                message.SetPublishError(result.FinalException?.ToString() ?? "Unknwon error");
                await supportNotifierService.AttemptNotifyAsync($"Cannot publish the OutboxMessage with Id {message.Id}", cancellationToken);
            }

            message.MarkProcessed();
            await outboxReader.MarkProcessedAsync(message, cancellationToken);
        }
    }
}