using Atlas.Law.Application.CQRS.LegalDocuments.Queries.Shared;
using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using AutoMapper;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Queries.GetUserLegalDocuments;

internal sealed class GetUserLegalDocumentsQueryHandler(ILegalDocumentRepository legalDocumentRepository, IMapper mapper) : IQueryHandler<GetUserLegalDocumentsQuery, IEnumerable<LegalDocumentDto>>
{
    public async Task<IEnumerable<LegalDocumentDto>> Handle(GetUserLegalDocumentsQuery request, CancellationToken cancellationToken)
    {
        return mapper.Map<IEnumerable<LegalDocumentDto>>(await legalDocumentRepository.GetByUserIdAsync(request.UserId, false, cancellationToken));
    }
}
