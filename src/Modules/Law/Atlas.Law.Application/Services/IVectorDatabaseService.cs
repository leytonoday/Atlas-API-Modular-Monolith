namespace Atlas.Law.Application.Services;

/// <summary>
/// Interface for interacting with a database service that deals with vectors.
/// </summary>
public interface IVectorDatabaseService
{
    /// <summary>
    /// Inserts a vector into the database asynchronously.
    /// </summary>
    /// <param name="vector">The vector to insert into the database.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task InsertVectorAsync(IEnumerable<float> vector, CancellationToken cancellationToken);

    /// <summary>
    /// Fetches a list of vector IDs where the vectors' values are similar to the provided vector.
    /// </summary>
    /// <param name="vector">The vector to find other similar vectors to.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A task representing the asynchronous operation that yields a list of vector IDs.</returns>
    public Task<IEnumerable<string>> GetSimilarVectorIdsAsync(IEnumerable<float> vector, CancellationToken cancellationToken);
}
