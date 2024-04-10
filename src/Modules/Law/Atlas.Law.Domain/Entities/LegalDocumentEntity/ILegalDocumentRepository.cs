using Atlas.Shared.Domain.Persistance;

namespace Atlas.Law.Domain.Entities.LegalDocumentEntity;

/// <summary>
/// Represents a repository for managing <see cref="LegalDocument"/> entities.
/// </summary>
public interface ILegalDocumentRepository : IRepository<LegalDocument, Guid>
{
    public Task<LegalDocument?> GetByNameAndUserAsync(string name, Guid userId, bool trackChanges, CancellationToken cancellationToken);

    public Task<IEnumerable<LegalDocument>> GetByUserIdAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);
}