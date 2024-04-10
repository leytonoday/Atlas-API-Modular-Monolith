using Atlas.Infrastructure.Persistance;
using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Law.Infrastructure.Persistance.Repositories;

public sealed class LegalDocumentRepository(LawDatabaseContext context)
    : Repository<LegalDocument, LawDatabaseContext, Guid>(context), ILegalDocumentRepository
{
    public async Task<LegalDocument?> GetByNameAndUserAsync(string name, Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<LegalDocument> legalDocuments = GetDbSet(trackChanges);

        return await legalDocuments.FirstOrDefaultAsync(x => x.UserId == userId && x.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<LegalDocument>> GetByUserIdAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<LegalDocument> legalDocuments = GetDbSet(trackChanges);

        return await legalDocuments.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
    }
}