using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;
using Atlas.Shared.Domain.Persistance;

namespace Atlas.Law.Domain.Entities.LegalDocumentEntity;

/// <summary>
/// Represents a repository for managing <see cref="LegalDocument"/> entities.
/// </summary>
public interface ILegalDocumentRepository : IRepository<LegalDocument, Guid>
{
    /// <summary>
    /// Retrieves a legal document by its name and user asynchronously.
    /// </summary>
    /// <param name="name">The name of the legal document to retrieve.</param>
    /// <param name="userId">The unique identifier of the user who owns the legal document.</param>
    /// <param name="trackChanges">A flag indicating whether to track changes for the retrieved entity.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the retrieved legal document, or null if not found.</returns>
    public Task<LegalDocument?> GetByNameAndUserAsync(string name, Guid userId, bool trackChanges, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all legal documents associated with a specified user asynchronously.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose legal documents are to be retrieved.</param>
    /// <param name="trackChanges">A flag indicating whether to track changes for the retrieved entities.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of legal documents.</returns>
    public Task<IEnumerable<LegalDocument>> GetByUserIdAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a legal document summary to the legal document asynchronously.
    /// </summary>
    /// <param name="legalDocumentSummary">The legal document summary to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task AddSummaryAsync(LegalDocumentSummary legalDocumentSummary, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a legal document summary from the legal document asynchronously.
    /// </summary>
    /// <param name="legalDocumentSummary">The legal document summary to remove.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task RemoveSummaryAsync(LegalDocumentSummary legalDocumentSummary, CancellationToken cancellationToken);
}