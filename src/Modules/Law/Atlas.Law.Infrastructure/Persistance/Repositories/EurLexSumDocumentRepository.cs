using Atlas.Infrastructure.Persistance;
using Atlas.Law.Domain.Entities.EurLexSumDocumentEntity;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Law.Infrastructure.Persistance.Repositories;

/// <inheritdoc cref="IEurLexSumDocumentRepository"/>
public sealed class EurLexSumDocumentRepository(LawDatabaseContext context)
    : Repository<EurLexSumDocument, LawDatabaseContext, Guid>(context), IEurLexSumDocumentRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<EurLexSumDocument>> GetByCelexId(string celexId, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<EurLexSumDocument> query = GetDbSet(trackChanges);

        return await query.Where(x => x.CelexId == celexId).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<EurLexSumDocument?> GetByCelexIdAndLanguage(string celexId, string language, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<EurLexSumDocument> query = GetDbSet(trackChanges);

        return await query.Where(x => x.CelexId == celexId && x.Language == language).FirstOrDefaultAsync(cancellationToken);
    }
}