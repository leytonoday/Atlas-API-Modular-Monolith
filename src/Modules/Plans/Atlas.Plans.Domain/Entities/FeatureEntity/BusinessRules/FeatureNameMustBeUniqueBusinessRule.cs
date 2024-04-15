using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Plans.Domain.Entities.FeatureEntity.BusinessRules;

/// <summary>
/// A business rule ensuring that a feature name is unique.
/// </summary>
/// <param name="name">The name of the feature.</param>
/// <param name="featureRepository">The repository to check for the existence of other features.</param>
internal class FeatureNameMustBeUniqueBusinessRule(string name, IFeatureRepository featureRepository) : IAsyncBusinessRule
{
    /// <inheritdoc/>
    public string Message => "The Feature name must be unique.";

    /// <inheritdoc/>
    public string Code => $"Feature.{nameof(FeatureNameMustBeUniqueBusinessRule)}";

    /// <inheritdoc/>
    public async Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
    {
        // Check if a feature with the same name already exists in the repository.
        Feature? existingFeature = await featureRepository.GetByNameAsync(name, false, cancellationToken);

        // Return true if a feature with the same name exists, otherwise return false.
        return existingFeature is not null;
    }
}
