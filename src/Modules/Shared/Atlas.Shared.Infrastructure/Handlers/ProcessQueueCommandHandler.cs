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

/// <summary>
/// Implements the <see cref="ICommandHandler{ProcessQueueCommand}"/> interface to process messages from a queue.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ProcessQueueCommandHandler"/> class.
/// </remarks>
/// <param name="commandsExecutor">The service used to execute commands.</param>
/// <param name="queueReader">The service used to read messages from the queue.</param>
/// <param name="logger">The logger for recording processing information.</param>
/// <param name="supportNotifierService">The service for sending notifications in case of processing errors.</param>
public sealed class ProcessQueueCommandHandler(ICommandsExecutor commandsExecutor, IQueueReader queueReader, ILogger<ProcessQueueCommandHandler> logger, ISupportNotifierService supportNotifierService) : ICommandHandler<ProcessQueueCommand>
{

    /// <summary>
    /// Defines a retry policy using Polly for handling exceptions during command execution.
    /// </summary>
    private static readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(
            3,
            attempt => TimeSpan.FromMilliseconds(100 * attempt)
        );

    /// <summary>
    /// Handles the <see cref="ProcessQueueCommand"/> by reading messages from the queue, processing them using the retry policy, and updating their status.
    /// </summary>
    /// <param name="request">The ProcessQueueCommand instance (typically empty).</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>An awaitable task.</returns>
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
                // Log the exception, mark message with error, and potentially notify
                logger.LogError(result.FinalException, "Cannot send the command for QueuedMessage with Id {messageId}", message.Id);

                await queueReader.MarkFailedAsync(message, result.FinalException?.ToString() ?? "Unknwon error", cancellationToken);

                await supportNotifierService.AttemptNotifyAsync($"Cannot send the command for QueuedMessage with Id {message.Id}", cancellationToken);
                continue;
            }

            await queueReader.MarkProcessedAsync(message, cancellationToken);
        }
    }
}
