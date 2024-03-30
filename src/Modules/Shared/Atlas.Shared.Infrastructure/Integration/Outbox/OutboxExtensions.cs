using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Integration.Outbox;

public static class OutboxExtensions
{
    public static void AddOutbox(this ModelBuilder modelBuilder, string schema)
    {
        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable(SharedInfrastructureConstants.TableNames.OutboxMessages, schema);
        });
    }
}
