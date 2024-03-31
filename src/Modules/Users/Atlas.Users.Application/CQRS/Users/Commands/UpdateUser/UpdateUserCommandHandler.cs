using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Application.Abstractions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandHandler(UserManager<User> userManager, IExecutionContextAccessor executionContext) : ICommandHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByIdAsync(executionContext.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        await User.UpdateAsync(user, request.UserName, request.PhoneNumber, userManager);
    }
}
