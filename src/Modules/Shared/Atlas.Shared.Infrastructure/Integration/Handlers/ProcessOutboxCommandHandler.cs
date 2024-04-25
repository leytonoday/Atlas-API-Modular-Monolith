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

/// <summary>
/// Implements the <see cref="ICommandHandler{ProcessOutboxCommand}"/> interface to process messages from an outbox.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ProcessOutboxCommandHandler"/> class.
/// </remarks>
/// <param name="outboxReader">The service used to read messages from the outbox.</param>
/// <param name="eventBus">The service used to publish events to the event bus.</param>
/// <param name="logger">The logger for recording processing information.</param>
/// <param name="supportNotifierService">The service for sending notifications in case of processing errors.</param>
public class ProcessOutboxCommandHandler(IOutboxReader outboxReader, IEventBus eventBus, ILogger<ProcessOutboxCommandHandler> logger, ISupportNotifierService supportNotifierService) : ICommandHandler<ProcessOutboxCommand>
{

    /// <summary>
    /// Defines a retry policy using Polly for handling exceptions during message publishing.
    /// </summary>
    private static readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(
            3,
            attempt => TimeSpan.FromMilliseconds(100 * attempt)
        );

    /// <summary>
    /// Handles the <see cref="ProcessOutboxCommand"/> by reading messages from the outbox, processing them using the retry policy, and updating their status.
    /// </summary>
    /// <param name="command">The ProcessOutboxCommand instance (typically empty).</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An awaitable task.</returns>
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
                // Skip message if it cannot be converted to an integration event
                continue;
            }

            PolicyResult result = await _retryPolicy.ExecuteAndCaptureAsync(() => eventBus.Publish(integrationEvent, cancellationToken));

            if (result.Outcome == OutcomeType.Failure)
            {
                // Log the exception, mark message with error, and potentially notify
                logger.LogError(result.FinalException, "Cannot publish the OutboxMessage with Id {messageId}", message.Id);
                
                await outboxReader.MarkFailedAsync(message, result.FinalException?.ToString() ?? "Unknown error", cancellationToken);

                await supportNotifierService.AttemptNotifyAsync($"Cannot publish the OutboxMessage with Id {message.Id}", cancellationToken);
                
                continue;
            }

            await outboxReader.MarkProcessedAsync(message, cancellationToken);
        }
    }
}
