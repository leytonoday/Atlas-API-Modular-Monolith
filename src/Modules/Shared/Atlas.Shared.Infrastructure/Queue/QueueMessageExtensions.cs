using Atlas.Shared.Application.Queue;
using Atlas.Shared.Infrastructure.Queue;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.CommandQueue;

public static class QueueMessageExtensions
{
    public static void AddCommandMessageQueue(this ModelBuilder modelBuilder, string schema)
    {
        modelBuilder.Entity<QueueMessage>(entity =>
        {
            entity.ToTable(SharedInfrastructureConstants.TableNames.QueueMessages, schema);
        });

        modelBuilder.Entity<QueueMessageHandlerAcknowledgement>(entity =>
        {
            // Set the primary key to be a composite key, setting a unique constraint of these two columns
            entity.HasKey(x => new { x.HandlerName, x.QueuedCommandId });

            entity.ToTable(SharedInfrastructureConstants.TableNames.QueueMessageHandlerAcknowledgements, schema);
        });
    }
}
