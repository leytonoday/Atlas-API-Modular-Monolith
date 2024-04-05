using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Commands;
using Atlas.Shared.Infrastructure.Integration.Inbox;
using Atlas.Shared.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly.Retry;
using Polly;
using Atlas.Shared.Application.Abstractions.Services;

namespace Atlas.Shared.Infrastructure.Integration.Handlers;

public class ProcessInboxCommandHandler(IInboxReader inboxReader, IPublisher publisher, ILogger<ProcessInboxCommandHandler> logger, ISupportNotifierService supportNotifierService) : ICommandHandler<ProcessInboxCommand>
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

    public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        List<InboxMessage> messages = await inboxReader.ListPendingAsync(cancellationToken);

        logger.LogDebug("Found {messagesCount} pending messages in inbox.", messages.Count);

        foreach (var message in messages)
        {
            logger.LogInformation($"Processing outbox message: {message.Id} {message.Type} {message.Data}");

            PolicyResult result = await _retryPolicy.ExecuteAndCaptureAsync(() => publisher.Publish(InboxMessage.ToIntegrationEvent(message), cancellationToken));

            if (result.Outcome == OutcomeType.Failure)
            {
                // Log the final exception and mark the queue message with the exception details
                logger.LogError(result.FinalException, "Cannot send the command for InboxMessage with Id {messageId}", message.Id);
                message.SetPublishError(result.FinalException?.ToString() ?? "Unknwon error");
                await supportNotifierService.AttemptNotifyAsync($"Cannot send the command for InboxMessage with Id {message.Id}", cancellationToken);
            }

            message.MarkProcessed();
            await inboxReader.MarkProcessedAsync(message, cancellationToken);
        }
    }
}