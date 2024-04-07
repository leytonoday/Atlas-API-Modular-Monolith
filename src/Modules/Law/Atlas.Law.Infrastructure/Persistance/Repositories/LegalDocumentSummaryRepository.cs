using Atlas.Infrastructure.Persistance;
using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;

namespace Atlas.Law.Infrastructure.Persistance.Repositories;

public sealed class LegalDocumentSummaryRepository(LawDatabaseContext context)
    : Repository<LegalDocumentSummary, LawDatabaseContext, Guid>(context), ILegalDocumentSummaryRepository;