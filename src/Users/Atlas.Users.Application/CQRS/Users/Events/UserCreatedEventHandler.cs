using Atlas.Shared.Application.Abstractions.Messaging;
using Atlas.Shared.Application.Abstractions.Services.EmailService;
using Atlas.Shared.Application.EmailContent;
using Atlas.Shared.Domain.Events.UserEvents;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Microsoft.AspNetCore.Identity;
using System.Web;

namespace Atlas.Users.Application.CQRS.Users.Events;

internal sealed class UserCreatedEventHandler(IEmailService emailService, UserManager<User> userManager) : IDomainEventHandler<UserCreatedEvent>
{
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByEmailAsync(notification.Email.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        // Create a token used to confirm the user's email address
        string confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await userManager.SetAuthenticationTokenAsync(user, "Default", "ConfirmUserEmail", confirmEmailToken);
        
        // Send email to user
        await emailService.SendEmailAsync(user.Email!, new ConfirmUserEmailEmailContent(user.UserName!, HttpUtility.UrlEncode(confirmEmailToken)), cancellationToken);
    }
}
