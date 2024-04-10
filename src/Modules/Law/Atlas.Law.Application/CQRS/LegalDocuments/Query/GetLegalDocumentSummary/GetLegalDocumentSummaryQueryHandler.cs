using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Query;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Query.GetLegalDocumentSummary;

internal sealed class GetLegalDocumentSummaryQueryHandler(ILegalDocumentSummaryRepository legalDocumentSummaryRepository) : IQueryHandler<GetLegalDocumentSummaryQuery, LegalDocumentSummary?>
{
    public async Task<LegalDocumentSummary?> Handle(GetLegalDocumentSummaryQuery request, CancellationToken cancellationToken)
    {
        return (await legalDocumentSummaryRepository.GetByConditionAsync(x => x.LegalDocumentId == request.LegalDocumentId, false, cancellationToken)).FirstOrDefault();
    }
}
