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

        User.CanSignInAsync(user);

        var passwordHasher = new PasswordHasher<User>();
        if (string.IsNullOrWhiteSpace(request.Password) || passwordHasher.VerifyHashedPassword(user!, user!.PasswordHash!, request.Password) != PasswordVerificationResult.Success)
        {
            throw new ErrorException(UsersDomainErrors.User.InvalidCredentials);
        }


        IEnumerable<string> roles = await userManager.GetRolesAsync(user);

        return new CanSignInResponse(true, user.Id, user.UserName, user.Email, roles);
    }
}
