using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Infrastructure.CQRS.PlanFeatures;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Domain.Exceptions;
using AutoMapper;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Queries.GetPlanFeaturesByPlanId;

internal sealed class GetPlanFeaturesByPlanIdQueryHandler(IPlanRepository planRepository, IMapper mapper) : IQueryHandler<GetPlanFeaturesByPlanIdQuery, IEnumerable<PlanFeatureDto>>
{
    public async Task<IEnumerable<PlanFeatureDto>> Handle(GetPlanFeaturesByPlanIdQuery request, CancellationToken cancellationToken)
    {
        Plan plan = await planRepository.GetByIdAsync(request.PlanId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        IEnumerable<PlanFeatureDto> planFeatures = mapper.Map<IEnumerable<PlanFeatureDto>>(plan.PlanFeatures);

        return mapper.Map<IEnumerable<PlanFeatureDto>>(planFeatures);
    }
}
