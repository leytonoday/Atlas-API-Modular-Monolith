using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Users.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler(UserManager<User> userManager, IUserRepository userRepository, IExecutionContextAccessor executionContext) : ICommandHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        User user = await userRepository.GetByIdAsync(executionContext.UserId, true, cancellationToken)
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        User.ChangePassword(user, request.OldPassword, request.NewPassword, userManager);

        await userRepository.UpdateAsync(user, cancellationToken);
    }
}
