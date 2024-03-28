using Atlas.Plans.Infrastructure.Persistance.Entities;
using Atlas.Shared.Infrastructure.Persistance.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Plans.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="PlansOutboxMessage"/> entity.
/// </summary>
public sealed class PlansOutboxMessageConfiguration : IEntityTypeConfiguration<PlansOutboxMessage>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="PlansOutboxMessage"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="PlansOutboxMessage"/> entity.</param>
    public void Configure(EntityTypeBuilder<PlansOutboxMessage> builder)
    {
        // Set the table name for this entity
        builder.ToTable(PlansConstants.TableNames.PlansOutboxMessages);

        // Set the primary key
        builder.HasKey(x => x.Id);
    }
}
