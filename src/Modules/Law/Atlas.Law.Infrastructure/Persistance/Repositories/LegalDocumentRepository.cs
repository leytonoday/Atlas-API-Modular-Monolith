using Atlas.Infrastructure.Persistance;
using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace Atlas.Law.Infrastructure.Persistance.Repositories;

/// <inheritdoc cref="ILegalDocumentRepository"/>
public sealed class LegalDocumentRepository(LawDatabaseContext context)
    : Repository<LegalDocument, LawDatabaseContext, Guid>(context), ILegalDocumentRepository
{
    /// <inheritdoc/>
    public override async Task<LegalDocument?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<LegalDocument> query = GetDbSet(trackChanges);

        return await query
            .Include(x => x.Summary)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<LegalDocument>> GetByConditionAsync(Expression<Func<LegalDocument, bool>>? condition, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<LegalDocument> query = GetDbSet(trackChanges);
        if (condition != null)
            query = query.Where(condition);

        return await query
            .Include(x => x.Summary)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<LegalDocument?> GetByNameAndUserAsync(string name, Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<LegalDocument> legalDocuments = GetDbSet(trackChanges);

        return await legalDocuments
            .Include(x => x.Summary)
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Name == name, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LegalDocument>> GetByUserIdAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<LegalDocument> legalDocuments = GetDbSet(trackChanges);

        return await legalDocuments
            .Include(x => x.Summary)
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task AddSummaryAsync(LegalDocumentSummary legalDocumentSummary, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Context.Set<LegalDocumentSummary>().Add(legalDocumentSummary);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task RemoveSummaryAsync(LegalDocumentSummary legalDocumentSummary, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Context.Set<LegalDocumentSummary>().Remove(legalDocumentSummary);
        return Task.CompletedTask;
    }
}