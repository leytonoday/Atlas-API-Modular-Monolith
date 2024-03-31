using Atlas.Plans.Application.CQRS.Plans.Shared;
using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Plans.Application.CQRS.Plans.Queries.GetPlanByUserId;

internal sealed class GetPlanByUserIdQueryHandler(UserManager<User> userManager, IPlanRepository planRepository, IMapper mapper) : IRequestHandler<GetPlanByUserIdQuery, PlanDto?>
{
    public async Task<PlanDto?> Handle(GetPlanByUserIdQuery request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);
        
        if (!user.PlanId.HasValue)
        {
            return null;
        }

        Plan plan = await planRepository.GetByIdAsync(user.PlanId.Value, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        return mapper.Map<PlanDto>(plan);
    }
}
