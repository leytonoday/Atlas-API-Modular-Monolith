using Atlas.Infrastructure.Persistance;
using Atlas.Law.Domain.Entities.LegalDocumentEntity;

namespace Atlas.Law.Infrastructure.Persistance.Repositories;

public sealed class LegalDocumentRepository(LawDatabaseContext context)
    : Repository<LegalDocument, LawDatabaseContext, Guid>(context), ILegalDocumentRepository;