using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Shared.Infrastructure.Integration.Inbox;
using Atlas.Shared.Infrastructure.Integration.Outbox;
using Atlas.Shared.Infrastructure.CommandQueue;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Plans.Infrastructure.Persistance;

public sealed class PlansDatabaseContext(DbContextOptions<PlansDatabaseContext> options) : DbContext(options)
{
    /// <summary>
    /// Configures the model for the database context during the model creation process.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the database context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(PlansConstants.Database.SchemaName);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlansDatabaseContext).Assembly);

        modelBuilder.AddInbox(PlansConstants.Database.SchemaName);
        modelBuilder.AddOutbox(PlansConstants.Database.SchemaName);
        modelBuilder.AddCommandMessageQueue(PlansConstants.Database.SchemaName);
    }

    /// <summary>
    /// Represents a collection of <see cref="Plan"/> in the context, or that can be queried from the database.
    /// </summary>
    public DbSet<Plan> Plans { get; set; }

    /// <summary>
    /// Represents a collection of <see cref="PlanFeature"/> in the context, or that can be queried from the database.
    /// </summary>
    public DbSet<PlanFeature> PlanFeatures { get; set; }

    /// <summary>
    /// Represents a collection of <see cref="Feature"/> in the context, or that can be queried from the database.
    /// </summary>
    public DbSet<Feature> Features { get; set; }

    /// <summary>
    /// Represents a collection of <see cref="StripeCustomer"/> in the context, or that can be queried from the database.
    /// </summary>
    public DbSet<StripeCustomer> StripeCustomers { get; set; }

    /// <summary>
    /// Represents a collection of <see cref="StripeCardFingerprint"/> in the context, or that can be queried from the database.
    /// </summary>
    public DbSet<StripeCardFingerprint> StripeCardFingerprints { get; set; }

    /// <summary>
    /// Represents a collection of <see cref="InboxMessage"/> in the context, or that can be queried from the database.
    /// </summary>
    public DbSet<InboxMessage> Inbox { get; set; }

    /// <summary>
    /// Represents a collection of <see cref="OutboxMessage"/> in the context, or that can be queried from the database.
    /// </summary>
    public DbSet<OutboxMessage> Outbox { get; set; }
}
