using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Errors;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Users.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Web;

namespace Atlas.Users.Application.CQRS.Authentication.Commands.SignInWithToken;

internal sealed class SignInWithTokenCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager) : ICommandHandler<SignInWithTokenCommand>
{
    public async Task Handle(SignInWithTokenCommand request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByUserNameOrEmailAsync(request.Identifier)
            ?? throw new ErrorException(UsersDomainErrors.User.InvalidCredentials);

        // Verify if the token is legit
        if (!await userManager.VerifyUserTokenAsync(user, "Default", UserToken.SignIn, HttpUtility.UrlDecode(request.Token)))
        {
            throw new ErrorException(UsersDomainErrors.User.InvalidToken);
        }

        // If token legit, remove it 
        await userManager.RemoveAuthenticationTokenAsync(user, "Default", UserToken.SignIn);

        if (!await signInManager.CanSignInAsync(user!))
        {
            if (user!.EmailConfirmed == false)
            {
                throw new ErrorException(UsersDomainErrors.User.MustVerifyEmail);
            }
            else
            {
                throw new ErrorException(Error.UnknownError);
            }

            // TODO - add logic for lockout as well, for account suspension
        }

        await signInManager.SignInAsync(user!, isPersistent: true);
    }
}
