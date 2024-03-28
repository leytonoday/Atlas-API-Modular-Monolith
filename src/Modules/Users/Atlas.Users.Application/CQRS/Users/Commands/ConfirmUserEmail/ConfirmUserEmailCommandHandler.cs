using Atlas.Shared.Domain.Events.UserEvents;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Users.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Web;

namespace Atlas.Users.Application.CQRS.Users.Commands.ConfirmUserEmail;

internal sealed class ConfirmUserEmailCommandHandler(UserManager<User> userManager) : IRequestHandler<ConfirmUserEmailCommand, string>
{
    public async Task<string> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByNameAsync(request.UserName)
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        if (user.EmailConfirmed)
        {
            throw new ErrorException(UsersDomainErrors.User.EmailAlreadyVerified);
        }

        user.AddDomainEvent(new UserEmailConfirmedEvent(Guid.NewGuid(), user.Id));

        // Confirm email using provided token
        IdentityResult result = await userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
        {
            throw new ErrorException(result.GetErrors());
        }

        // Delete the confirm email token that was generated upon sign up
        await userManager.RemoveAuthenticationTokenAsync(user, "Default", UserToken.ConfirmUserEmail);

        // Create a new token that can be used to sign in the user 
        string signInToken = await userManager.GenerateUserTokenAsync(user, "Default", UserToken.SignIn);

        return HttpUtility.UrlEncode(signInToken);
    }
}
