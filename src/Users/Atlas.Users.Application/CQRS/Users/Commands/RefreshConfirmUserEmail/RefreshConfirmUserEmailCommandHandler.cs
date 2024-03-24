using Atlas.Shared.Application.Abstractions.Services.EmailService;
using Atlas.Shared.Application.EmailContent;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Users.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Users.Commands.RefreshConfirmUserEmail;

internal sealed class RefreshConfirmUserEmailCommandHandler(UserManager<User> userManager, IEmailService emailService) : IRequestHandler<RefreshConfirmUserEmailCommand>
{
    public async Task Handle(RefreshConfirmUserEmailCommand request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByUserNameOrEmailAsync(request.Identifier)
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        // Create a token used to confirm the user's email address
        string confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await userManager.SetAuthenticationTokenAsync(user, "Default", UserToken.ConfirmUserEmail, confirmEmailToken);

        // Send email to user
        await emailService.SendEmailAsync(user.Email!, new ConfirmUserEmailEmailContent(user.UserName!, confirmEmailToken), cancellationToken);
    }
}
