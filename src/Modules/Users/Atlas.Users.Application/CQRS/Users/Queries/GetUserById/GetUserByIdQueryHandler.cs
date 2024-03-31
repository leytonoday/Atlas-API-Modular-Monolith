using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Application.CQRS.Users.Shared;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Infrastructure.CQRS.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(UserManager<User> userManager, IMapper mapper) : IQueryHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByIdAsync(request.Id.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        UserDto userResponse = mapper.Map<UserDto>(user);

        userResponse.Roles = await userManager.GetRolesAsync(user);

        return userResponse;
    }
}
