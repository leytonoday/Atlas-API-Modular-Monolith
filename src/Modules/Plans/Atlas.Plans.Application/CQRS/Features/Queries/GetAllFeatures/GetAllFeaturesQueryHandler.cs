using Atlas.Plans.Application.CQRS.Features.Shared;
using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using AutoMapper;
using MediatR;

namespace Atlas.Plans.Infrastructure.CQRS.Features.Queries.GetAllFeatures;

public sealed class GetAllFeaturesQueryHandler(IFeatureRepository featureRepository, IMapper mapper) : IRequestHandler<GetAllFeaturesQuery, IEnumerable<FeatureDto>>
{
    public async Task<IEnumerable<FeatureDto>> Handle(GetAllFeaturesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Feature> features = await featureRepository.GetAllAsync(false, cancellationToken);
        return mapper.Map<IEnumerable<FeatureDto>>(features);
    }
}