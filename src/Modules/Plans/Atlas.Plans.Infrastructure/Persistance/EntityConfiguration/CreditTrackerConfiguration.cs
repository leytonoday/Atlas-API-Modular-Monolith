using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Atlas.Plans.Domain.Entities.CreditTrackerEntity;

namespace Atlas.Plans.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="CreditTracker"/> entity.
/// </summary>
internal sealed class CreditTrackerConfiguration : IEntityTypeConfiguration<CreditTracker>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="CreditTracker"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="CreditTracker"/> entity.</param>
    public void Configure(EntityTypeBuilder<CreditTracker> builder)
    {
        // Set the table name for this entity
        builder.ToTable(PlansConstants.TableNames.CreditTrackers);

        // Set the primary key
        builder.HasKey(x => x.UserId);
    }
}