using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Integration.Outbox;

/// <summary>
/// Extension methods for configuring the outbox functionality in Entity Framework Core.
/// </summary>
public static class OutboxExtensions
{
    /// <summary>
    /// Configures the outbox functionality for the specified model builder.
    /// </summary>
    /// <param name="modelBuilder">The model builder instance.</param>
    /// <param name="schema">The database schema to use for the outbox table.</param>
    public static void AddOutbox(this ModelBuilder modelBuilder, string schema)
    {
        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            // Configures the table name for storing outbox messages.
            entity.ToTable(SharedInfrastructureConstants.TableNames.OutboxMessages, schema);
        });
    }
}
