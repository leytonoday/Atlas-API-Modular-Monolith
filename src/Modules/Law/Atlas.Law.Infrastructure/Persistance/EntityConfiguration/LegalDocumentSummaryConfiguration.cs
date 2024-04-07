using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;

namespace Atlas.Law.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="LegalDocumentSummary"/> entity.
/// </summary>
internal sealed class LegalDocumentSummaryConfiguration : IEntityTypeConfiguration<LegalDocumentSummary>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="LegalDocumentSummary"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="LegalDocumentSummary"/> entity.</param>
    public void Configure(EntityTypeBuilder<LegalDocumentSummary> builder)
    {
        // Set the table name for this entity
        builder.ToTable(LawConstants.TableNames.LegalDocumentSummaries);

        // Set the primary key
        builder.HasKey(x => x.Id);
    }
}
