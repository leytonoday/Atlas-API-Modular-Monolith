using Atlas.Shared.Domain.Errors;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Users.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Authentication.Commands.SignIn;

internal sealed class SignInCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager) : IRequestHandler<SignInCommand>
{
    public async Task Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByUserNameOrEmailAsync(request.Identifier)
            ?? throw new ErrorException(UsersDomainErrors.User.InvalidCredentials);

        var passwordHasher = new PasswordHasher<User>();
        if (string.IsNullOrWhiteSpace(request.Password) || passwordHasher.VerifyHashedPassword(user!, user!.PasswordHash!, request.Password) != PasswordVerificationResult.Success)
        {
            throw new ErrorException(UsersDomainErrors.User.InvalidCredentials);
        }

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
