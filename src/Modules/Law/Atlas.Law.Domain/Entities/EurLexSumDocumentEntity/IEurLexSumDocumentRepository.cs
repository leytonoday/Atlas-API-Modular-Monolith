using Atlas.Shared.Domain.Persistance;

namespace Atlas.Law.Domain.Entities.EurLexSumDocumentEntity;

/// <summary>
/// Represents a repository for managing <see cref="EurLexSumDocument"/> entities.
/// </summary>
public interface IEurLexSumDocumentRepository : IRepository<EurLexSumDocument, Guid>
{
    public Task<IEnumerable<EurLexSumDocument>> GetByCelexId(string celexId, bool trackChagnes, CancellationToken cancellationToken);

    public Task<EurLexSumDocument?> GetByCelexIdAndLanguage(string celexId, string language, bool trackChanges, CancellationToken cancellationToken);
}