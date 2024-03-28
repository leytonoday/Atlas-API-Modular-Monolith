using Atlas.Shared.Application.Abstractions.Services;
using Atlas.Shared.Domain.Events;
using Atlas.Shared.Infrastructure.Persistance.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Quartz;

namespace Atlas.Infrastructure.BackgroundJobs;

/// <summary>
/// Represents a Quartz.NET job responsible for processing outbox messages by publishing their associated domain events.
/// This job retrieves a batch of unprocessed outbox messages from the database, deserializes the domain events,
/// and attempts to publish them using a provided message publisher.
/// </summary>
[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob<TDatabaseContext, TOutboxMessage>(TDatabaseContext dbContext, IPublisher publisher, ILogger<ProcessOutboxMessagesJob<TDatabaseContext, TOutboxMessage>> logger, ISupportNotifierService supportNotifierService) : IJob
    where TDatabaseContext : DbContext
    where TOutboxMessage : OutboxMessage
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

    /// <summary>
    /// Executes the processing logic for the outbox messages. Retrieves a batch of unprocessed outbox messages,
    /// deserializes their domain events, and attempts to publish them using the provided publisher. Applies the
    /// retry policy to handle transient failures during the publishing process.
    /// </summary>
    /// <param name="context">The execution context provided by Quartz.NET.</param>
    public async Task Execute(IJobExecutionContext context)
    {
        // Get the latest 20 unpublished outbox messages
        List<TOutboxMessage> outboxMessages = await dbContext
            .Set<TOutboxMessage>()
            .Where(x => x.ProcessedOnUtc == null && x.PublishError == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(100)
            .ToListAsync();

        if (outboxMessages.Count == 0)
        {
            return;
        }

        // Deserialize each one and publish them.
        foreach (TOutboxMessage outboxMessage in outboxMessages)
        {
            // Using Newtonsoft.Json because of a lack of support for polymorphic deserialisation within System.Text.Json 
            IDomainEvent? domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(
                    outboxMessage.Data,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            if (domainEvent is null)
            {
                logger.LogError("Cannot deserialise Domain Event for OutboxMessage with Id {outboxMessageId}", outboxMessage.Id);
                continue;
            }

            // Attempt to publish the method multiple times incase of failures within the event handlers
            PolicyResult result = await _retryPolicy.ExecuteAndCaptureAsync(() => 
                publisher.Publish(domainEvent, context.CancellationToken));

            if (result.Outcome == OutcomeType.Failure)
            {
                // Log the final exception and mark the outbox message with the exception details
                logger.LogError(result.FinalException, "Cannot publish Domain Event for OutboxMessage with Id {outboxMessageId}", outboxMessage.Id);
                outboxMessage.PublishError = result.FinalException?.ToString();
                await supportNotifierService.AttemptNotifyAsync($"Cannot publish Domain Event for OutboxMessage with Id {outboxMessage.Id}");
            }

            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        logger.LogInformation("Successfully processed {outboxMessagesCount} Outbox Messages", outboxMessages.Count);

        await dbContext.SaveChangesAsync();
    }
}
