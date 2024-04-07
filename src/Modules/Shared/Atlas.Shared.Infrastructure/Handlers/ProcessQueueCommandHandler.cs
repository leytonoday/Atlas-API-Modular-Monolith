using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Abstractions.Services;
using Atlas.Shared.Application.Commands;
using Atlas.Shared.Application.Queue;
using Atlas.Shared.Infrastructure.Module;
using Atlas.Shared.Infrastructure.Queue;
using Microsoft.Extensions.Logging;
using Polly.Retry;
using Polly;

namespace Atlas.Shared.Infrastructure.Handlers;

public sealed class ProcessQueueCommandHandler(ICommandsExecutor commandsExecutor, IQueueReader queueReader, ILogger<ProcessQueueCommandHandler> logger, ISupportNotifierService supportNotifierService) : ICommandHandler<ProcessQueueCommand>
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

    public async Task Handle(ProcessQueueCommand request, CancellationToken cancellationToken)
    {
        List<QueueMessage> messages = await queueReader.ListPendingAsync(cancellationToken);

        logger.LogDebug("Found {messagesCount} pending messages in queue.", messages.Count);

        foreach (var message in messages)
        {
            logger.LogInformation($"Processing queue message: {message.Id} {message.Type} {message.Data}");

            PolicyResult result = await _retryPolicy.ExecuteAndCaptureAsync(() => commandsExecutor.SendCommand(QueueMessage.ToRequest(message), cancellationToken));

            if (result.Outcome == OutcomeType.Failure)
            {
                // Log the final exception and mark the queue message with the exception details
                logger.LogError(result.FinalException, "Cannot send the command for QueuedMessage with Id {messageId}", message.Id);
                message.SetError(result.FinalException?.ToString() ?? "Unknwon error");
                await supportNotifierService.AttemptNotifyAsync($"Cannot send the command for QueuedMessage with Id {message.Id}", cancellationToken);
            }

            message.MarkProcessed();
            await queueReader.MarkProcessedAsync(message, cancellationToken);
        }
    }
}