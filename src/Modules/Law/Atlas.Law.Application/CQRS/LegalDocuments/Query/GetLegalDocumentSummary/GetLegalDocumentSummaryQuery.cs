using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Query;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Query.GetLegalDocumentSummary;

public record GetLegalDocumentSummaryQuery(Guid LegalDocumentId) : IQuery<LegalDocumentSummary?>;
