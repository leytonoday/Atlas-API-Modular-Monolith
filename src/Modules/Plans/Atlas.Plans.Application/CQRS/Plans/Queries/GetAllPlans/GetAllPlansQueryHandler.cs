using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Services;
using AutoMapper;
using MediatR;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Infrastructure.CQRS.PlanFeatures;
using Atlas.Plans.Application.CQRS.Features.Shared;
using Atlas.Plans.Application.CQRS.Plans.Shared;
using Atlas.Shared.Application.Abstractions.Messaging.Query;

namespace Atlas.Plans.Application.CQRS.Plans.Queries.GetAllPlans;

internal sealed class GetAllPlansQueryHandler(IPlanRepository planRepository, PlanService planService, IMapper mapper) : IQueryHandler<GetAllPlansQuery, IEnumerable<PlanDto>>
{
    public async Task<IEnumerable<PlanDto>> Handle(GetAllPlansQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Plan> plans = await planRepository.GetAllAsync(false, cancellationToken);

        plans = plans.Where(x => request.IncludeInactive || x.Active).OrderBy(x => x.MonthlyPrice).ToList();

        IEnumerable<PlanDto> planDtos = plans.Select(mapper.Map<PlanDto>).ToList();

        foreach (PlanDto planDto in planDtos)
        {
            Plan plan = plans.First(x => x.Id == planDto.Id);
            planDto.InheritedPlanFeatures = mapper.Map<IEnumerable<PlanFeatureDto>>(await planService.GetInheritedPlanFeatures(plan, cancellationToken));
            planDto.InheritedFeatures = mapper.Map<IEnumerable<FeatureDto>>(await planService.GetInheritedFeatures(plan, cancellationToken));
        }

        return planDtos;
    }
}
