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
    }
}
