using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Microsoft.AspNetCore.Identity;
using Atlas.Users.Domain.Extensions;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Users.Application.CQRS.Authentication.Queries.Shared;

namespace Atlas.Users.Application.CQRS.Authentication.Queries.CanSignIn;

internal sealed class CanSignInQueryHandler(UserManager<User> userManager) : IQueryHandler<CanSignInQuery, CanSignInResponse>
{
    public async Task<CanSignInResponse> Handle(CanSignInQuery request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByUserNameOrEmailAsync(request.Identifier)
            ?? throw new ErrorException(UsersDomainErrors.User.InvalidCredentials);

        var passwordHasher = new PasswordHasher<User>();
        if (string.IsNullOrWhiteSpace(request.Password) || passwordHasher.VerifyHashedPassword(user!, user!.PasswordHash!, request.Password) != PasswordVerificationResult.Success)
        {
            throw new ErrorException(UsersDomainErrors.User.InvalidCredentials);
        }

        // TODO - Convert this to a check on the user with a series of Business Rules
        //if (!await signInManager.CanSignInAsync(user!))
        //{
        //    if (user!.EmailConfirmed == false)
        //    {
        //        throw new ErrorException(UsersDomainErrors.User.MustVerifyEmail);
        //    }
        //    else
        //    {
        //        throw new ErrorException(Error.UnknownError);
        //    }

        //    // TODO - add logic for lockout as well, for account suspension
        //}

        IEnumerable<string> roles = await userManager.GetRolesAsync(user);

        return new CanSignInResponse(true, user.Id, user.UserName, user.Email, roles);
    }
}
