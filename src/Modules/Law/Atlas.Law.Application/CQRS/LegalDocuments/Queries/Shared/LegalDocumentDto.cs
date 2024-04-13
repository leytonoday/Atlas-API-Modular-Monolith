namespace Atlas.Law.Application.CQRS.LegalDocuments.Queries.Shared;

public class LegalDocumentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string FullText { get; set; }
    public string Language { get; set; }
    public string MimeType { get; set; }
    public LegalDocumentSummaryDto? Summary { get; set; }
}