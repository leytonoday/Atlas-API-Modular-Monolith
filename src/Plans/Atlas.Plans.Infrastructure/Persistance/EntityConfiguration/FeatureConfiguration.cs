using Atlas.Plans.Domain.Entities.FeatureEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Plans.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="Feature"/> entity.
/// </summary>
internal sealed class FeatureConfiguration : IEntityTypeConfiguration<Feature>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="Feature"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="Feature"/> entity.</param>
    public void Configure(EntityTypeBuilder<Feature> builder)
    {
        // Set the table name for this entity
        builder.ToTable(PlansTableNames.Features);

        // Set the primary key
        builder.HasKey(x => x.Id);

        // Add database level constraint to ensure feature name is unique
        builder.HasIndex(x => x.Name).IsUnique();
    }
}
