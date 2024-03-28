using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Plans.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="Plan"/> entity.
/// </summary>
internal sealed class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="Plan"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="Plan"/> entity.</param>
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        // Set the table name for this entity
        builder.ToTable(PlansConstants.TableNames.Plans);

        // Set the primary key
        builder.HasKey(x => x.Id);

        // Add database level constraint to ensure feature name is unique
        builder.HasIndex(x => x.Name).IsUnique();

        // Many to many relationship between plans and features, using PlanFeature as a join table
        builder.HasMany(x => x.Features).WithMany(x => x.Plans).UsingEntity<PlanFeature>();
    }
}
