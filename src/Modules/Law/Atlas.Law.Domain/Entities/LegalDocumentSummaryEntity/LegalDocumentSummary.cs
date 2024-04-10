using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Law.Domain.Enums;
using Atlas.Shared.Domain.Entities;

namespace Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;

public class LegalDocumentSummary : Entity<Guid>
{
    private LegalDocumentSummary() { }

    public string? SummarisedText { get; private set; }

    public string? SummarizedTitle { get; private set; }

    public string? Keywords { get; private set; }

    public Guid LegalDocumentId { get; private set; }

    public LegalDocument LegalDocument { get; private set; }

    public LegalDocumentProcessingStatus ProcessingStatus { get; private set; }

    public static LegalDocumentSummary Create(Guid LegalDocumentId)
    {
        return new LegalDocumentSummary()
        {
            LegalDocumentId = LegalDocumentId,
            ProcessingStatus = LegalDocumentProcessingStatus.NOT_STARTED
        };
    }

    public static void SetSummary(LegalDocumentSummary legalDocumentSummary, string summarisedText, string summarisedTitle, string summarisedKeywords)
    {
        legalDocumentSummary.SummarisedText = summarisedText;
        legalDocumentSummary.Keywords = summarisedKeywords;
        legalDocumentSummary.SummarizedTitle = summarisedTitle;
        legalDocumentSummary.ProcessingStatus = LegalDocumentProcessingStatus.COMPLETE;
    }

    public static void SetAsProcessing(LegalDocumentSummary legalDocumentSummary)
    {
        legalDocumentSummary.ProcessingStatus = LegalDocumentProcessingStatus.PROCESSING;
    }
}
