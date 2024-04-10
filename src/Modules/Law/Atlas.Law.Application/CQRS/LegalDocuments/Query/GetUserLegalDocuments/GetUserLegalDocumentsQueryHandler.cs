using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Query;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Query.GetUserLegalDocuments;

internal sealed class GetUserLegalDocumentsQueryHandler(ILegalDocumentRepository legalDocumentRepository) : IQueryHandler<GetUserLegalDocumentsQuery, IEnumerable<LegalDocument>>
{
    public async Task<IEnumerable<LegalDocument>> Handle(GetUserLegalDocumentsQuery request, CancellationToken cancellationToken)
    {
        return await legalDocumentRepository.GetByUserIdAsync(request.UserId, false, cancellationToken);
    }
}
