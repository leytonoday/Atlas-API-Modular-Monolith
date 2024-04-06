using Atlas.Plans.Application.CQRS.Features.Shared;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using AutoMapper;

namespace Atlas.Plans.Infrastructure.CQRS.Features.Queries.GetAllFeatures;

internal sealed class GetAllFeaturesQueryHandler(IFeatureRepository featureRepository, IMapper mapper) : IQueryHandler<GetAllFeaturesQuery, IEnumerable<FeatureDto>>
{
    public async Task<IEnumerable<FeatureDto>> Handle(GetAllFeaturesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Feature> features = await featureRepository.GetAllAsync(false, cancellationToken);
        return mapper.Map<IEnumerable<FeatureDto>>(features);
    }
}