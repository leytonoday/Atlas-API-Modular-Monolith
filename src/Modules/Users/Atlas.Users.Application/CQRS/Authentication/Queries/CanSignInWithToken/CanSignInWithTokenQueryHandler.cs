using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Microsoft.AspNetCore.Identity;
using Atlas.Users.Domain.Extensions;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Users.Application.CQRS.Authentication.Queries.Shared;
using System.Web;
using Atlas.Users.Application.CQRS.Authentication.Queries.CanSignInWithToken;

namespace Atlas.Users.Application.CQRS.Authentication.Queries.CanSignIn;

internal sealed class CanSignInWithTokenQueryHandler(UserManager<User> userManager) : IQueryHandler<CanSignInWithTokenQuery, CanSignInResponse>
{
    public async Task<CanSignInResponse> Handle(CanSignInWithTokenQuery request, CancellationToken cancellationToken)
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
