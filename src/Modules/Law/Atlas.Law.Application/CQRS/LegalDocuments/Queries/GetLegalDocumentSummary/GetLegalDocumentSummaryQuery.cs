using Atlas.Law.Application.CQRS.LegalDocuments.Queries.Shared;
using Atlas.Shared.Application.Abstractions.Messaging.Query;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Queries.GetLegalDocumentSummary;

public record GetLegalDocumentSummaryQuery(Guid LegalDocumentId) : IQuery<LegalDocumentSummaryDto?>;
