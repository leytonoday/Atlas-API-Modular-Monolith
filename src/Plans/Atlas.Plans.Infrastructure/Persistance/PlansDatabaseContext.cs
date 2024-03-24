using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Shared.Infrastructure.Persistance;
using Atlas.Shared.Infrastructure.Persistance.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Plans.Infrastructure.Persistance;

internal sealed class PlansDatabaseContext(DbContextOptions options) : DbContext(options), IDatabaseContext
{
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
    /// Represents a collection of <see cref="OutboxMessage"/> in the context, or that can be queried from the database.
    /// </summary>
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    /// <summary>
    /// Represents a collection of <see cref="OutboxMessageConsumerAcknowledgement"/> in the context, or that can be queried from the database.
    /// </summary>
    public DbSet<OutboxMessageConsumerAcknowledgement> OutboxMessageConsumerAcknowledgements { get; set; }
}
