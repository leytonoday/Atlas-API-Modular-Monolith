using Atlas.Shared.Application.Queue;
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
    }
}
