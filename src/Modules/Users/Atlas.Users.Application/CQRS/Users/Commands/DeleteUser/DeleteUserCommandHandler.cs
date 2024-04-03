using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler(UserManager<User> userManager, IUserRepository userRepository, IExecutionContextAccessor executionContext) : ICommandHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User currentUser = await userRepository.GetByIdAsync(executionContext.UserId, true, cancellationToken)
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        User userToBeDeleted = await userRepository.GetByIdAsync(request.UserId, true, cancellationToken)
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        await User.DeleteUserAsync(currentUser, userToBeDeleted, request.Password, userManager);

        await userRepository.RemoveAsync(userToBeDeleted, cancellationToken);

        // After the user has been deleted, sign then out manually.
        await userManager.UpdateSecurityStampAsync(userToBeDeleted);
    }
}
