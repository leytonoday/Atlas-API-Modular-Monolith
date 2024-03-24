using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Plans.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="PlanFeature"/> entity.
/// </summary>
public class PlanFeatureConfiguration : IEntityTypeConfiguration<PlanFeature>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="PlanFeature"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="PlanFeature"/> entity.</param>
    public void Configure(EntityTypeBuilder<PlanFeature> builder)
    {
        // Set the table name for this entity
        builder.ToTable(PlansTableNames.PlanFeatures);

        // Composite key between feature id and plan id
        builder.HasKey(x => new { x.FeatureId, x.PlanId });
    }
}
