using Atlas.Shared.Application.Abstractions.Integration.Outbox;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Users.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Web;
using Atlas.Users.IntegrationEvents;

namespace Atlas.Users.Application.CQRS.Users.Commands.ConfirmUserEmail;

internal sealed class ConfirmUserEmailCommandHandler(UserManager<User> userManager, IUserRepository userRepository, IOutboxWriter outboxWriter) : ICommandHandler<ConfirmUserEmailCommand, string>
{
    public async Task<string> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
    {
        User user = await userRepository.GetByUserNameAsync(request.UserName, true, cancellationToken)
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        await User.ConfirmEmailAsync(user, request.Token, userManager);

        // Delete the confirm email token that was generated upon sign up
        await userManager.RemoveAuthenticationTokenAsync(user, "Default", UserToken.ConfirmUserEmail);

        // Create a new token that can be used to sign in the user 
        string signInToken = await userManager.GenerateUserTokenAsync(user, "Default", UserToken.SignIn);

        // Alert external systems
        await outboxWriter.WriteAsync(new UserEmailConfirmedIntegrationEvent(user.Id, user.UserName, user.Email, user.PhoneNumber), cancellationToken);
        
        return HttpUtility.UrlEncode(signInToken);
    }
}
