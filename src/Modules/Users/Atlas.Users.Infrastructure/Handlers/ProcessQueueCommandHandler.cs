using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Commands;
using Atlas.Shared.Application.Queue;
using Atlas.Shared.Infrastructure.Module;
using Atlas.Shared.Infrastructure.Queue;
using Atlas.Users.Module;
using Microsoft.Extensions.Logging;

namespace Atlas.Users.Infrastructure.Handlers;

public sealed class ProcessQueueCommandHandler(IQueueReader queueReader, ILogger<ProcessQueueCommandHandler> logger) : ICommandHandler<ProcessQueueCommand>
{
    public async Task Handle(ProcessQueueCommand request, CancellationToken cancellationToken)
    {
        List<QueueMessage> messages = await queueReader.ListPendingAsync(cancellationToken);

        logger.LogDebug("Found {messagesCount} pending messages in queue.", messages.Count);

        foreach (var message in messages)
        {
            logger.LogInformation($"Processing outbox message: {message.Id} {message.Type} {message.Data}");
            await CommandsExecutor<UsersCompositionRoot>.SendCommand(QueueMessage.ToRequest(message), cancellationToken);
            message.MarkProcessed();
            await queueReader.MarkProcessedAsync(message, cancellationToken);
        }
    }
}