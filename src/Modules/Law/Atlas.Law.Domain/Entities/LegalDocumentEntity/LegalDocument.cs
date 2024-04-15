using Atlas.Law.Domain.Entities.LegalDocumentEntity.BusinessRules;
using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;
using Atlas.Shared.Domain.AggregateRoot;
using Atlas.Shared.Domain.Entities;

namespace Atlas.Law.Domain.Entities.LegalDocumentEntity;

/// <summary>
/// Represents a legal document.
/// </summary>
public sealed class LegalDocument : Entity<Guid>, IAggregateRoot
{
    private LegalDocument() { }

    /// <summary>
    /// Gets or sets the name of the legal document.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the full text content of the legal document.
    /// </summary>
    public string FullText { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the language of the legal document.
    /// </summary>
    public string Language { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the MIME type of the legal document, indicating the type of file originally used when uploading the legal document.
    /// </summary>
    public string MimeType { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier of the user who owns the legal document.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets or sets the summary associated with the legal document.
    /// </summary>
    public LegalDocumentSummary? Summary { get; private set; }

    /// <summary>
    /// Creates a new legal document asynchronously.
    /// </summary>
    /// <param name="name">The name of the legal document.</param>
    /// <param name="fullText">The full text content of the legal document.</param>
    /// <param name="language">The language of the legal document.</param>
    /// <param name="mimeType">The MIME type of the legal document.</param>
    /// <param name="userId">The unique identifier of the user who owns the legal document.</param>
    /// <param name="legalDocumentRepository">The repository for managing legal documents.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created legal document.</returns>
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

    /// <summary>
    /// Deletes a legal document asynchronously.
    /// </summary>
    /// <param name="legalDocument">The legal document to delete.</param>
    /// <param name="legalDocumentRepository">The repository for managing legal documents.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task DeleteAsync(
        LegalDocument legalDocument,
        ILegalDocumentRepository legalDocumentRepository,
        CancellationToken cancellationToken)
    {
        CheckBusinessRule(new LegalDocumentCannotBeDeletedWhilstSummaryIncompleteBusinessRule(legalDocument));

        await legalDocumentRepository.RemoveAsync(legalDocument, cancellationToken);
    }

    /// <summary>
    /// Creates a summary for the legal document asynchronously.
    /// </summary>
    /// <param name="legalDocument">The legal document for which to create the summary.</param>
    /// <param name="legalDocumentRepository">The repository for managing legal documents.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created legal document summary.</returns>
    public static async Task<LegalDocumentSummary> CreateSummaryAsync(
        LegalDocument legalDocument,
        ILegalDocumentRepository legalDocumentRepository,
        CancellationToken cancellationToken)
    {
        var summary = LegalDocumentSummary.Create(legalDocument.Id);
        await legalDocumentRepository.AddSummaryAsync(summary, cancellationToken);

        LegalDocumentSummary.SetAsProcessing(summary);

        return summary;
    }

    /// <summary>
    /// Removes the summary associated with the legal document asynchronously.
    /// </summary>
    /// <param name="legalDocument">The legal document for which to remove the summary.</param>
    /// <param name="legalDocumentRepository">The repository for managing legal documents.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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
