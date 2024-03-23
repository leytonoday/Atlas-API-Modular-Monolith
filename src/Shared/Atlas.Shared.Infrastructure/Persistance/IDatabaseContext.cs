using Atlas.Shared.Infrastructure.Persistance.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Shared.Infrastructure.Persistance;

public interface IDatabaseContext
{
    public DbSet<OutboxMessageConsumerAcknowledgement> OutboxMessageConsumerAcknowledgements { get; set; }
}
