using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Commands;
using Atlas.Shared.Infrastructure.Integration.Inbox;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly.Retry;
using Polly;
using Atlas.Shared.Application.Abstractions.Services;

namespace Atlas.Shared.Infrastructure.Integration.Handlers;

/// <summary>
/// Implements the <see cref="ICommandHandler{ProcessInboxCommand}"/> interface to process messages from an inbox.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ProcessInboxCommandHandler"/> class.
/// </remarks>
/// <param name="inboxReader">The service used to read messages from the inbox.</param>
/// <param name="publisher">The service used to publish messages from the inbox.</param>
/// <param name="logger">The logger for recording processing information.</param>
/// <param name="supportNotifierService">The service for sending notifications in case of processing errors.</param>
public class ProcessInboxCommandHandler(IInboxReader inboxReader, IPublisher publisher, ILogger<ProcessInboxCommandHandler> logger, ISupportNotifierService supportNotifierService) : ICommandHandler<ProcessInboxCommand>
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
    /// Handles the <see cref="ProcessInboxCommand"/> by reading messages from the inbox, processing them using the retry policy, and updating their status.
    /// </summary>
    /// <param name="command">The ProcessInboxCommand instance (typically empty).</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An awaitable task.</returns>
    public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        List<InboxMessage> messages = await inboxReader.ListPendingAsync(cancellationToken);

        logger.LogDebug("Found {messagesCount} pending messages in inbox.", messages.Count);

        foreach (var message in messages)
        {
            logger.LogInformation($"Processing inbox message: {message.Id} {message.Type} {message.Data}");

            PolicyResult result = await _retryPolicy.ExecuteAndCaptureAsync(() => publisher.Publish(InboxMessage.ToIntegrationEvent(message), cancellationToken));

            if (result.Outcome == OutcomeType.Failure)
            {
                // Log the exception, mark message with error, and potentially notify
                logger.LogError(result.FinalException, "Cannot publish message for InboxMessage with Id {messageId}", message.Id);
                message.SetPublishError(result.FinalException?.ToString() ?? "Unknown error");
                await supportNotifierService.AttemptNotifyAsync($"Cannot publish message for InboxMessage with Id {message.Id}", cancellationToken);
            }

            message.MarkProcessed();
            await inboxReader.MarkProcessedAsync(message, cancellationToken);
        }
    }
}
