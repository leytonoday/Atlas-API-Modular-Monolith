using Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Plans.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="StripeCardFingerprint"/> entity.
/// </summary>
internal sealed class StripeCardFingerprintConfiguration : IEntityTypeConfiguration<StripeCardFingerprint>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="StripeCardFingerprint"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="StripeCardFingerprint"/> entity.</param>
    public void Configure(EntityTypeBuilder<StripeCardFingerprint> builder)
    {
        // Set the table name for this entity
        builder.ToTable(PlansConstants.TableNames.StripeCardFingerprints);

        // Set the primary key
        builder.HasKey(x => x.Fingerprint);
    }
}
