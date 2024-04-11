using Atlas.Law.Domain.Entities.LegalDocumentEntity.BusinessRules;
using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;
using Atlas.Law.Domain.Enums;
using Atlas.Shared.Domain.AggregateRoot;

namespace Atlas.Law.Domain.Entities.LegalDocumentEntity;

public sealed class LegalDocument : AggregateRoot<Guid>
{
    private LegalDocument() { }

    public string Name { get; private set; } = null!;

    public string FullText { get; private set; } = null!;

    public string Language { get; private set; } = null!;

    public string MimeType { get; private set; } = null!;

    public Guid UserId { get; private set; }

    public static async Task<LegalDocument> CreateAsync(string name, string fullText, string language, string mimeType, Guid userId, ILegalDocumentRepository legalDocumentRepository)
    {
        Guid id = Guid.NewGuid();

        var legalDocument = new LegalDocument
        {
            Id = id,
            Name = name,
            FullText = fullText,
            UserId = userId,
            Language = language,
            MimeType = mimeType,
        };

        await CheckAsyncBusinessRule(new LegalDocumentNameMustBeUniqueBusinessRule(name, userId, legalDocumentRepository));

        return legalDocument;
    }

    public static async Task DeleteAsync(
        LegalDocument legalDocument, 
        ILegalDocumentRepository legalDocumentRepository,
        ILegalDocumentSummaryRepository legalDocumentSummaryRepository,
        CancellationToken cancellationToken)
    {
        await CheckAsyncBusinessRule(new LegalDocumentCannotBeDeletedWhilstSummaryIncomplete(legalDocument.Id, legalDocumentSummaryRepository), cancellationToken);

        await legalDocumentRepository.RemoveAsync(legalDocument, cancellationToken);
    }
}