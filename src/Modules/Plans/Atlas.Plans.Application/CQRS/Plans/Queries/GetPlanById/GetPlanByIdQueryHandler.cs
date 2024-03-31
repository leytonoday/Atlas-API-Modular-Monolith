using Atlas.Plans.Application.CQRS.Plans.Shared;
using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Domain.Exceptions;
using AutoMapper;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Queries.GetPlanById;

internal sealed class GetPlanByIdQueryHandler(IPlanRepository planRepository, IMapper mapper) : IQueryHandler<GetPlanByIdQuery, PlanDto>
{
    public async Task<PlanDto> Handle(GetPlanByIdQuery request, CancellationToken cancellationToken)
    {
        Plan plan = await planRepository.GetByIdAsync(request.Id, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        return mapper.Map<PlanDto>(plan);
    }
}
