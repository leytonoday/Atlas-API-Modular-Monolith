using Atlas.Law.Application.CQRS.LegalDocuments.Queries.Shared;
using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Law.Domain.Errors;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Domain.Exceptions;
using AutoMapper;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Queries.GetLegalDocumentSummary;

internal sealed class GetLegalDocumentSummaryQueryHandler(ILegalDocumentRepository legalDocumentRepository, IMapper mapper) : IQueryHandler<GetLegalDocumentSummaryQuery, LegalDocumentSummaryDto?>
{
    public async Task<LegalDocumentSummaryDto?> Handle(GetLegalDocumentSummaryQuery request, CancellationToken cancellationToken)
    {
        LegalDocument legalDocument = await legalDocumentRepository.GetByIdAsync(request.LegalDocumentId, false, cancellationToken)
            ?? throw new ErrorException(LawDomainErrors.Law.LegalDocumentNotFound);

        return mapper.Map<LegalDocumentSummaryDto?>(legalDocument.Summary);
    }
}
