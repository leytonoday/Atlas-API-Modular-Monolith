using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Application.Abstractions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Users.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Users.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler(UserManager<User> userManager, IUserContext userContext, SignInManager<User> signInManager) : ICommandHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByIdAsync(userContext.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        IdentityResult result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            throw new ErrorException(result.GetErrors());
        }
        // Once password has been reset, user must essentially be re-logged in
        await signInManager.RefreshSignInAsync(user);
    }
}
