using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Infrastructure.CQRS.PlanFeatures;
using AutoMapper;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Queries.GetPlanFeaturesByPlanId;

internal sealed class GetPlanFeaturesByPlanIdQueryHandler(IPlanFeatureRepository planFeatureRepository, IMapper mapper) : IRequestHandler<GetPlanFeaturesByPlanIdQuery, IEnumerable<PlanFeatureDto>>
{
    public async Task<IEnumerable<PlanFeatureDto>> Handle(GetPlanFeaturesByPlanIdQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<PlanFeatureDto> planFeatures = mapper.Map<IEnumerable<PlanFeatureDto>>(await planFeatureRepository.GetByPlanId(request.PlanId, false, cancellationToken));

        return mapper.Map<IEnumerable<PlanFeatureDto>>(planFeatures);
    }
}
