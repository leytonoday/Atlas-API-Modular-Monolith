using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Application.Abstractions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler(UserManager<User> userManager, IUserContext userContext) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User currentUser = await userManager.FindByIdAsync(userContext.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        User userToBeDeleted = await User.DeleteUserAsync(currentUser, request.UserId, request.Password, userManager);

        // After the user has been deleted, sign then out manually.
        await userManager.UpdateSecurityStampAsync(userToBeDeleted);
    }
}
