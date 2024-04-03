using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Plans.Domain.Entities.FeatureEntity.BusinessRules;

internal class FeatureNameMustBeUniqueBusinessRule(string name, IFeatureRepository featureRepository) : IAsyncBusinessRule
{
    public string Message => "The Feature name must be unique.";

    public async Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
    {
        Feature? existingFeature = await featureRepository.GetByNameAsync(name, false, cancellationToken);
        return existingFeature is not null;
    }
}
