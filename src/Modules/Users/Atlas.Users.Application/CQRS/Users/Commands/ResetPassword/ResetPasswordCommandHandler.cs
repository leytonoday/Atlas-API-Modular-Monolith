﻿using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Users.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Users.Commands.ResetPassword;

internal sealed class ResetPasswordCommandHandler(UserManager<User> userManager) : ICommandHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByNameAsync(request.UserName)
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        await User.ResetPasswordAsync(user, request.NewPassword, request.Token, userManager);

        await userManager.RemoveAuthenticationTokenAsync(user, "Default", UserToken.ResetPassword);
    }
}
