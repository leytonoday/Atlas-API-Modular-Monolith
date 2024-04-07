using Microsoft.EntityFrameworkCore;
using Atlas.Shared.Infrastructure.Integration.Inbox;
using Atlas.Shared.Infrastructure.Integration.Outbox;
using Atlas.Shared.Infrastructure.CommandQueue;

namespace Atlas.Law.Infrastructure.Persistance;

public sealed class LawDatabaseContext : DbContext
{
    public LawDatabaseContext(DbContextOptions<LawDatabaseContext> options) : base(options) { }

    /// <summary>
    /// Configures the model for the database context during the model creation process.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the database context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(LawConstants.Database.SchemaName);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LawDatabaseContext).Assembly);

        modelBuilder.AddInbox(LawConstants.Database.SchemaName);
        modelBuilder.AddOutbox(LawConstants.Database.SchemaName);
        modelBuilder.AddCommandMessageQueue(LawConstants.Database.SchemaName);
    }
}
