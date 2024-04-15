using Atlas.Shared.Domain.AggregateRoot;
using Atlas.Shared.Domain.Entities;

namespace Atlas.Law.Domain.Entities.EurLexSumDocumentEntity;

/// <summary>
/// Represents a legal document taken from the EUR-Lex-Sum dataset (https://arxiv.org/abs/2210.13448)
/// </summary>
public sealed class EurLexSumDocument : Entity<Guid>, IAggregateRoot
{
    private EurLexSumDocument() { }

    /// <summary>
    /// The Celex ID is a naming convention used for identifying EU-related documents. Among other things, the year of publication and sector codes are embedded in the Celex ID.
    /// </summary>
    public string CelexId { get; private set; } = null!;

    /// <summary>
    /// This is the full text of a Legal Act published by the EU.
    /// </summary>
    public string Reference { get; private set; } = null!;

    /// <summary>
    /// This field contains the summary associated with the respective Legal Act.
    /// </summary>
    public string Summary { get; private set; } = null!;

    /// <summary>
    /// The language of the documument.
    /// </summary>
    public string Language { get; private set; } = null!;

    /// <summary>
    /// A series of keywords that represent this document.
    /// </summary>
    public string? Keywords { get; private set; }
}
