using Atlas.Shared.Application.Abstractions.Messaging.Queue;
using Atlas.Shared.Application.Abstractions.Services.EmailService;
using Atlas.Shared.Application.EmailContent;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Microsoft.AspNetCore.Identity;
using System.Web;

namespace Atlas.Users.Application.CQRS.Users.Queue.SendWelcomeEmail;

internal class SendWelcomeEmailQueuedCommandHandler(IEmailService emailService, UserManager<User> userManager) : IQueuedCommandHandler<SendWelcomeEmailQueuedCommand>
{
    public async Task Handle(SendWelcomeEmailQueuedCommand request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        // Create a token used to confirm the user's email address
        string confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await userManager.SetAuthenticationTokenAsync(user, "Default", "ConfirmUserEmail", confirmEmailToken);

        // Send email to user
        await emailService.SendEmailAsync(user.Email!, new ConfirmUserEmailEmailContent(user.UserName!, HttpUtility.UrlEncode(confirmEmailToken)), cancellationToken);
    }
}
