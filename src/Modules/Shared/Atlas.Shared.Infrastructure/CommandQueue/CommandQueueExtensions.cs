using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.CommandQueue;

public static class CommandQueueMessageExtensions
{
    public static void AddCommandMessageQueue(this ModelBuilder modelBuilder, string schema)
    {
        modelBuilder.Entity<CommandQueueMessage>(entity =>
        {
            entity.ToTable(SharedInfrastructureConstants.TableNames.CommandQueueMessages, schema);
        });
    }
}
