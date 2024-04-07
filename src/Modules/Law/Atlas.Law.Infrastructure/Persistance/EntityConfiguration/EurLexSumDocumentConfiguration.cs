using Atlas.Law.Domain.Entities.EurLexSumDocumentEntity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Law.Infrastructure.Persistance.EntityConfiguration;

/// <summary>
/// Configures entity mappings for the <see cref="EurLexSumDocument"/> entity.
/// </summary>
internal sealed class EurLexSumDocumentConfiguration : IEntityTypeConfiguration<EurLexSumDocument>
{
    /// <summary>
    /// Configures entity mappings for the <see cref="EurLexSumDocument"/> entity.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the <see cref="EurLexSumDocument"/> entity.</param>
    public void Configure(EntityTypeBuilder<EurLexSumDocument> builder)
    {
        // Set the table name for this entity
        builder.ToTable(LawConstants.TableNames.EurLexSumDocuments);

        // Set the primary key
        builder.HasKey(x => x.Id);

        // Unique index constraint on the celexId and the language. i.e., only one copy of a document can exist for each language
        builder.HasIndex(x => new { x.CelexId, x.Language }).IsUnique();
    }
}
