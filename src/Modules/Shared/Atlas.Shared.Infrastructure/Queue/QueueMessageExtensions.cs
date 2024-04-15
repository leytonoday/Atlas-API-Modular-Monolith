using Atlas.Shared.Application.Queue;
using Atlas.Shared.Infrastructure.Queue;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.CommandQueue;

/// <summary>
/// Extension methods for configuring message queues in Entity Framework Core.
/// </summary>
public static class QueueMessageExtensions
{
    /// <summary>
    /// Adds configurations for command message queues to the specified model builder.
    /// </summary>
    /// <param name="modelBuilder">The model builder instance.</param>
    /// <param name="schema">The database schema to use for the message queues.</param>
    public static void AddCommandMessageQueue(this ModelBuilder modelBuilder, string schema)
    {
        modelBuilder.Entity<QueueMessage>(entity =>
        {
            // Configures the table for storing queue messages.
            entity.ToTable(SharedInfrastructureConstants.TableNames.QueueMessages, schema);
        });

        modelBuilder.Entity<QueueMessageHandlerAcknowledgement>(entity =>
        {
            // Sets the primary key to be a composite key, ensuring uniqueness of the combination of HandlerName and QueuedCommandId.
            entity.HasKey(x => new { x.HandlerName, x.QueuedCommandId });

            // Configures the table for storing queue message handler acknowledgements.
            entity.ToTable(SharedInfrastructureConstants.TableNames.QueueMessageHandlerAcknowledgements, schema);
        });
    }
}
