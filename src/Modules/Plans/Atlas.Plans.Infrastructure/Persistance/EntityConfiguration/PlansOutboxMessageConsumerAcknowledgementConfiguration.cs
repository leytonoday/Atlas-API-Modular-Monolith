using Atlas.Plans.Infrastructure.Persistance.Entities;
using Atlas.Shared.Infrastructure.Persistance.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Plans.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="PlanOutboxMessageConsumerAcknowledgement"/> entity.
/// </summary>
public sealed class PlansOutboxMessageConsumerAcknowledgementConfiguration : IEntityTypeConfiguration<PlansOutboxMessageConsumerAcknowledgement>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="PlansOutboxMessageConsumerAcknowledgement"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="PlansOutboxMessageConsumerAcknowledgement"/> entity.</param>
    public void Configure(EntityTypeBuilder<PlansOutboxMessageConsumerAcknowledgement> builder)
    {
        // Set the table name for this entity
        builder.ToTable(PlansConstants.TableNames.PlansOutboxMessageConsumerAcknowledgements);

        // Set the primary key to be a composite key, setting a unique constraint of these two columns
        builder.HasKey(x => new
        {
            x.DomainEventId,
            x.EventHandlerName
        });
    }
}
