using Atlas.Plans.Application.CQRS.Plans.Shared;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Application.ModuleBridge;
using Atlas.Shared.Domain.Exceptions;
using AutoMapper;

namespace Atlas.Plans.Application.CQRS.Plans.Queries.GetPlanByUserId;

internal sealed class GetPlanByUserIdQueryHandler(IPlanRepository planRepository, IMapper mapper, IModuleBridge moduleBridge) : IQueryHandler<GetPlanByUserIdQuery, PlanDto?>
{
    public async Task<PlanDto?> Handle(GetPlanByUserIdQuery request, CancellationToken cancellationToken)
    {
        Guid? planId = await moduleBridge.GetUserPlanId(request.UserId, cancellationToken);

        if (!planId.HasValue)
        {
            return null;
        }

        Plan plan = await planRepository.GetByIdAsync(planId.Value, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        return mapper.Map<PlanDto>(plan);
    }
}
