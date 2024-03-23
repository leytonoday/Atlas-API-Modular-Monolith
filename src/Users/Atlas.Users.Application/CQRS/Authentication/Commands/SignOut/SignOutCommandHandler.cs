using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Application.Abstractions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Authentication.Commands.SignOut;

internal sealed class SignOutCommandHandler(SignInManager<User> signInManager, IUserContext userContext) : IRequestHandler<SignOutCommand>
{
    public async Task Handle(SignOutCommand signOutCommand, CancellationToken cancellationToken)
    {
        if (!userContext.IsAuthenticated)
        {
            throw new ErrorException(UsersDomainErrors.Authentication.NotAuthenticated);
        }

        await signInManager.SignOutAsync();
    }
}
