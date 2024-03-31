using Atlas.Shared.Application.Abstractions.Integration.Outbox;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Queue;
using Atlas.Shared.Domain.Events.UserEvents;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Users.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Web;
using Atlas.Users.IntegrationEvents;

namespace Atlas.Users.Application.CQRS.Users.Commands.ConfirmUserEmail;

internal sealed class ConfirmUserEmailCommandHandler(UserManager<User> userManager, IOutboxWriter outboxWriter) : ICommandHandler<ConfirmUserEmailCommand, string>
{
    public async Task<string> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByNameAsync(request.UserName)
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        if (user.EmailConfirmed)
        {
            throw new ErrorException(UsersDomainErrors.User.EmailAlreadyVerified);
        }

        await outboxWriter.WriteAsync(new UserEmailConfirmedIntegrationEvent(user.Id), cancellationToken);

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
