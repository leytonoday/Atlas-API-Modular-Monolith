using Atlas.Shared.Domain.Persistance;

namespace Atlas.Law.Domain.Entities.EurLexSumDocumentEntity;

/// <summary>
/// Represents a repository for managing <see cref="EurLexSumDocument"/> entities.
/// </summary>
public interface IEurLexSumDocumentRepository : IRepository<EurLexSumDocument, Guid>
{
    /// <summary>
    /// Retrieves a collection of <see cref="EurLexSumDocument"/> entities by their CELEX identifier.
    /// </summary>
    /// <param name="celexId">The CELEX identifier to search for.</param>
    /// <param name="trackChanges">A flag indicating whether to track changes for the retrieved entities.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of <see cref="EurLexSumDocument"/> entities.</returns>
    public Task<IEnumerable<EurLexSumDocument>> GetByCelexId(string celexId, bool trackChagnes, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a <see cref="EurLexSumDocument"/> entity by its CELEX identifier and language.
    /// </summary>
    /// <param name="celexId">The CELEX identifier to search for.</param>
    /// <param name="language">The language of the document.</param>
    /// <param name="trackChanges">A flag indicating whether to track changes for the retrieved entity.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="EurLexSumDocument"/> entity, or null if not found.</returns>
    public Task<EurLexSumDocument?> GetByCelexIdAndLanguage(string celexId, string language, bool trackChanges, CancellationToken cancellationToken);
}