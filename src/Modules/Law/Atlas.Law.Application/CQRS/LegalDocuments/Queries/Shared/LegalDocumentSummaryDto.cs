using Atlas.Law.Domain.Enums;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Queries.Shared;

public class LegalDocumentSummaryDto
{
    public Guid Id { get; set; }
    public string? SummarisedText { get; set; }
    public string? SummarizedTitle { get; set; }
    public string? Keywords { get; set; }
    public Guid LegalDocumentId { get; set; }

    public LegalDocumentProcessingStatus ProcessingStatus { get; set; }
}