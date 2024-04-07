using Atlas.Law.Domain.Enums;
using Atlas.Shared.Domain.AggregateRoot;

namespace Atlas.Law.Domain.Entities.LegalDocumentEntity;

public sealed class LegalDocument : AggregateRoot<Guid>
{
    private LegalDocument() { } 

    public string FullText { get; private set; } = null!;

    public string Language { get; private set; } = null!;

    public Guid UserId { get; private set; }

    public static LegalDocument Create(string fullText, string language, Guid userId)
    {
        Guid id = Guid.NewGuid();

        var legalDocument = new LegalDocument
        {
            Id = id,
            FullText = fullText,
            UserId = userId,
            Language = language
        };

        return legalDocument;
    }
}