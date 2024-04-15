using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Integration.Inbox;

/// <summary>
/// Provides extension methods for adding inbox-related entities to a DbContext model.
/// </summary>
public static class InboxExtensions
{
    /// <summary>
    /// Adds the <see cref="InboxMessage"/> entity to the DbContext model, specifying the table name and schema.
    /// </summary>
    /// <param name="modelBuilder">The DbContext model builder.</param>
    /// <param name="schema">The schema name for the inbox tables.</param>
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
