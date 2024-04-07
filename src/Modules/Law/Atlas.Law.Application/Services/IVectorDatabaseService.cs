namespace Atlas.Law.Application.Services;

public interface IVectorDatabaseService
{
    public Task InsertVectorAsync(IEnumerable<float> vector, CancellationToken cancellationToken);

    /// <summary>
    /// Fetches a list of vector ids where the vector's values are similar to <paramref name="vector"/>.
    /// </summary>
    /// <param name="vector">The vector to find other similar vectors to.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A list of vector ids.</returns>
    public Task<IEnumerable<string>> GetSimilarVectorIdsAsync(IEnumerable<float> vector, CancellationToken cancellationToken);
}
