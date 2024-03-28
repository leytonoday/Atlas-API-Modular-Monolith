using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Infrastructure.CQRS.Users.Queries.GetRolesByUserId;

internal sealed class GetRolesByUserIdQueryHandler(UserManager<User> userManager) : IRequestHandler<GetRolesByUserIdQuery, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(GetRolesByUserIdQuery request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        return await userManager.GetRolesAsync(user);
    }
}
