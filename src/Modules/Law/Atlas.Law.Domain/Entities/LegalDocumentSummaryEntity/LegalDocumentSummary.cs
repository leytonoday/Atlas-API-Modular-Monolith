using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Law.Domain.Enums;
using Atlas.Shared.Domain.Entities;

namespace Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;

/// <summary>
/// Represents a summary associated with a <see cref="LegalDocument"/>.
/// </summary>
public class LegalDocumentSummary : Entity<Guid>
{
    private LegalDocumentSummary() { }

    /// <summary>
    /// Gets or sets the summarised text of the legal document.
    /// </summary>
    public string? SummarisedText { get; private set; }

    /// <summary>
    /// Gets or sets the summarised title of the legal document.
    /// </summary>
    public string? SummarizedTitle { get; private set; }

    /// <summary>
    /// Gets or sets the keywords associated with the legal document summary.
    /// </summary>
    public string? Keywords { get; private set; }

    /// <summary>
    /// Gets or sets the unique identifier of the legal document associated with the summary.
    /// </summary>
    public Guid LegalDocumentId { get; private set; }

    /// <summary>
    /// Gets or sets the legal document associated with the summary.
    /// </summary>
    public LegalDocument LegalDocument { get; private set; }

    /// <summary>
    /// Gets or sets the processing status of the legal document summary. Legal document summaries are created as NOT_STARTED by default.
    /// The user has to manually trigger the summarisation process. This tracks that process.
    /// </summary>
    public LegalDocumentProcessingStatus ProcessingStatus { get; private set; }

    /// <summary>
    /// Creates a new legal document summary.
    /// </summary>
    /// <param name="legalDocumentId">The unique identifier of the legal document associated with the summary.</param>
    /// <returns>The created legal document summary.</returns>
    public static LegalDocumentSummary Create(Guid legalDocumentId)
    {
        return new LegalDocumentSummary()
        {
            LegalDocumentId = legalDocumentId,
            ProcessingStatus = LegalDocumentProcessingStatus.NotStarted
        };
    }

    /// <summary>
    /// Sets the summary details for the legal document summary.
    /// </summary>
    /// <param name="legalDocumentSummary">The legal document summary to update.</param>
    /// <param name="summarisedText">The summarised text of the legal document.</param>
    /// <param name="summarisedTitle">The summarised title of the legal document.</param>
    /// <param name="summarisedKeywords">The keywords associated with the legal document summary.</param>
    public static void SetSummary(LegalDocumentSummary legalDocumentSummary, string summarisedText, string summarisedTitle, string summarisedKeywords)
    {
        legalDocumentSummary.SummarisedText = summarisedText;
        legalDocumentSummary.Keywords = summarisedKeywords;
        legalDocumentSummary.SummarizedTitle = summarisedTitle;
        legalDocumentSummary.ProcessingStatus = LegalDocumentProcessingStatus.Complete;
    }

    /// <summary>
    /// Sets the legal document summary as processing.
    /// </summary>
    /// <param name="legalDocumentSummary">The legal document summary to update.</param>
    public static void SetAsProcessing(LegalDocumentSummary legalDocumentSummary)
    {
        legalDocumentSummary.ProcessingStatus = LegalDocumentProcessingStatus.Processing;
    }
}
