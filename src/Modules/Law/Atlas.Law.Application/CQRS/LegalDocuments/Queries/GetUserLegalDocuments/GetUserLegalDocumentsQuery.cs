using Atlas.Law.Application.CQRS.LegalDocuments.Queries.Shared;
using Atlas.Shared.Application.Abstractions.Messaging.Query;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Queries.GetUserLegalDocuments;

public sealed record GetUserLegalDocumentsQuery(Guid UserId) : IQuery<IEnumerable<LegalDocumentDto>>;
