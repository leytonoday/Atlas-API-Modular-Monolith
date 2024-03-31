using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Abstractions.Services.EmailService;
using Atlas.Shared.Application.EmailContent;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Users.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Web;

namespace Atlas.Users.Application.CQRS.Users.Commands.ForgotPassword;

internal sealed class ForgotPasswordCommandHandler(UserManager<User> userManager, IEmailService emailService) : ICommandHandler<ForgotPasswordCommand>
{
    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByUserNameOrEmailAsync(request.Identifier)
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        string passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user!);
        await userManager.SetAuthenticationTokenAsync(user!, "Default", UserToken.ResetPassword, passwordResetToken);

        // Send email to user
        await emailService.SendEmailAsync(user.Email!, new ResetPasswordEmailContent(user.UserName!, HttpUtility.UrlEncode(passwordResetToken)), cancellationToken);
    }
}
