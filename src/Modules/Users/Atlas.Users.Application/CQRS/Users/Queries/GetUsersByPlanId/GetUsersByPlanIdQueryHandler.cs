using Atlas.Users.Application.CQRS.Users.Queries.GetUsersByPlanId;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Users.Application.CQRS.Users.Shared;
using Atlas.Users.Domain.Entities.UserEntity;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Users.Application.CQRS.Users.Queries.GetByPlanId;

internal sealed class GetUsersByPlanIdQueryHandler(UserManager<User> userManager, IMapper mapper) : IQueryHandler<GetUsersByPlanIdQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(GetUsersByPlanIdQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<User> users = await userManager.Users.Where(x => x.PlanId == request.PlanId).ToListAsync(cancellationToken);
        return mapper.Map<IEnumerable<UserDto>>(users);
    }
}
