using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;

namespace Atlas.Law.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="LegalDocument"/> entity.
/// </summary>
internal sealed class LegalDocumentConfiguration : IEntityTypeConfiguration<LegalDocument>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="LegalDocument"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="LegalDocument"/> entity.</param>
    public void Configure(EntityTypeBuilder<LegalDocument> builder)
    {
        // Set the table name for this entity
        builder.ToTable(LawConstants.TableNames.LegalDocuments);

        // Set the primary key
        builder.HasKey(x => x.Id);

        // Has an optional one to one relationship with LegalDocumentSummary
        builder.HasOne(x => x.Summary).WithOne(x => x.LegalDocument).HasForeignKey<LegalDocumentSummary>(x => x.LegalDocumentId).IsRequired(false).OnDelete(DeleteBehavior.Cascade);
    }
}
