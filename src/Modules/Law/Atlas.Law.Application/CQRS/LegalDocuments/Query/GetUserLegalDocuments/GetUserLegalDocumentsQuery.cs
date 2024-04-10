using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Query;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Query.GetUserLegalDocuments;

public sealed record GetUserLegalDocumentsQuery(Guid UserId) : IQuery<IEnumerable<LegalDocument>>;
