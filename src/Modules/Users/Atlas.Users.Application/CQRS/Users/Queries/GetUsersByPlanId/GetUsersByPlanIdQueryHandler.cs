using Atlas.Users.Application.CQRS.Users.Queries.GetUsersByPlanId;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Users.Application.CQRS.Users.Shared;
using Atlas.Users.Domain.Entities.UserEntity;
using AutoMapper;

namespace Atlas.Users.Application.CQRS.Users.Queries.GetByPlanId;

internal sealed class GetUsersByPlanIdQueryHandler(IUserRepository userRepository, IMapper mapper) : IQueryHandler<GetUsersByPlanIdQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(GetUsersByPlanIdQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<User> users = await userRepository.GetByConditionAsync(x => x.PlanId == request.PlanId, false, cancellationToken);
        return mapper.Map<IEnumerable<UserDto>>(users);
    }
}
