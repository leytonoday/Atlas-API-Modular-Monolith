using Atlas.Shared.Infrastructure.Persistance.Outbox;
using Atlas.Users.Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Users.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="UsersOutboxMessageConsumerAcknowledgement"/> entity.
/// </summary>
internal sealed class OutboxMessageConsumerAcknowledgementConfiguration : IEntityTypeConfiguration<UsersOutboxMessageConsumerAcknowledgement>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="UsersOutboxMessageConsumerAcknowledgement"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="UsersOutboxMessageConsumerAcknowledgement"/> entity.</param>
    public void Configure(EntityTypeBuilder<UsersOutboxMessageConsumerAcknowledgement> builder)
    {
        // Set the table name for this entity
        builder.ToTable(UsersConstants.TableNames.UsersOutboxMessageConsumerAcknowledgements);

        // Set the primary key to be a composite key, setting a unique constraint of these two columns
        builder.HasKey(x => new
        {
            x.DomainEventId,
            x.EventHandlerName
        });
    }
}
