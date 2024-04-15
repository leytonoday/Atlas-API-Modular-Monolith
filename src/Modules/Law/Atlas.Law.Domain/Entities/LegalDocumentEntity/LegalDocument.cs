using Atlas.Law.Domain.Entities.LegalDocumentEntity.BusinessRules;
using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;
using Atlas.Shared.Domain.AggregateRoot;
using Atlas.Shared.Domain.Entities;

namespace Atlas.Law.Domain.Entities.LegalDocumentEntity;

public sealed class LegalDocument : Entity<Guid>, IAggregateRoot
{
    private LegalDocument() { }

    public string Name { get; private set; } = null!;

    public string FullText { get; private set; } = null!;

    public string Language { get; private set; } = null!;

    public string MimeType { get; private set; } = null!;

    public Guid UserId { get; private set; }

    public LegalDocumentSummary? Summary { get; private set; }

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
        CancellationToken cancellationToken)
    {
        CheckBusinessRule(new LegalDocumentCannotBeDeletedWhilstSummaryIncompleteBusinessRule(legalDocument));

        await legalDocumentRepository.RemoveAsync(legalDocument, cancellationToken);
    }

    public static async Task<LegalDocumentSummary> CreateSummaryAsync(
        LegalDocument legalDocument,
        ILegalDocumentRepository legalDocumentRepository,
        CancellationToken cancellationToken)
    {
        var summary = LegalDocumentSummary.Create(legalDocument.Id);
        await legalDocumentRepository.AddSummaryAsync(summary, cancellationToken);

        // Mark it as processing by default, so that if the generation of the summary takes a long time, and the user queries for the status of the job, they can see it's in the works.
        LegalDocumentSummary.SetAsProcessing(summary);

        return summary;
    }

    public static async Task RemoveSummaryAsync(
        LegalDocument legalDocument,
        ILegalDocumentRepository legalDocumentRepository,
        CancellationToken cancellationToken)
    {
        if (legalDocument.Summary is null)
        {
            return;
        }

        await legalDocumentRepository.RemoveSummaryAsync(legalDocument.Summary, cancellationToken);
    }
}