using Atlas.Shared.Domain.Persistance;

namespace Atlas.Plans.Domain.Entities.FeatureEntity;

/// <summary>
/// Represents a repository for managing <see cref="Feature"/> entities.
/// </summary>
public interface IFeatureRepository : IRepository<Feature, Guid>
{
    public Task<Feature?> GetByNameAsync(string name, bool trackChanges, CancellationToken cancellationToken);

    public Task<Feature?> GetByCodeAsync(string code, bool trackChanges, CancellationToken cancellationToken);
}