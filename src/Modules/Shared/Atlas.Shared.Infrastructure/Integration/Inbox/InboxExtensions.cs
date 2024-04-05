using Atlas.Shared.Infrastructure.Queue;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Integration.Inbox;

public static class InboxExtensions
{
    public static void AddInbox(this ModelBuilder modelBuilder, string schema)
    {
        modelBuilder.Entity<InboxMessage>(entity =>
        {
            entity.ToTable(SharedInfrastructureConstants.TableNames.InboxMessages, schema);
        });

        modelBuilder.Entity<InboxMessageHandlerAcknowledgement>(entity =>
        {
            // Set the primary key to be a composite key, setting a unique constraint of these two columns
            entity.HasKey(x => new { x.HandlerName, x.InboxMessageId });

            entity.ToTable(SharedInfrastructureConstants.TableNames.InboxMessageHandlerAcknowledgements, schema);
        });
    }
}
