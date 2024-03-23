using Atlas.Users.Domain.Entities.UserEntity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Users.Commands.CreateUser;

internal sealed class CreateUserCommandHandler(UserManager<User> userManager) : IRequestHandler<CreateUserCommand, string>
{
    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User user = await User.CreateAsync(request.UserName, request.Email, request.Password, userManager);

        return user.Id.ToString();
    }
}
