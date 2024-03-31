﻿using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Authentication.Commands.SignOut;

internal sealed class SignOutCommandHandler(SignInManager<User> signInManager, IExecutionContextAccessor executionContext) : ICommandHandler<SignOutCommand>
{
    public async Task Handle(SignOutCommand signOutCommand, CancellationToken cancellationToken)
    {
        if (!executionContext.IsUserAuthenticated)
        {
            throw new ErrorException(UsersDomainErrors.Authentication.NotAuthenticated);
        }

        await signInManager.SignOutAsync();
    }
}
