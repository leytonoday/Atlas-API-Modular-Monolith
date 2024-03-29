﻿using Atlas.Shared.Application.Abstractions.Services.EmailService;
using Atlas.Shared.Application.EmailContent;
using Atlas.Shared.Infrastructure.Razor.Views.Emails.ConfirmUserEmail;
using Atlas.Shared.Infrastructure.Razor.Views.Emails.ResetPasswordEmail;
using Atlas.Shared.Infrastructure.Razor.Views.Emails.SupportNotificationEmail;
using Atlas.Shared.Infrastructure.Services;

namespace Atlas.Shared.Infrastructure.Builders;

/// <summary>
/// Builds email content asynchronously using Razor views for templating.
/// </summary>
/// <param name="_razorViewToStringRenderer"></param>
public class EmailContentBuilder(RazorViewToStringRenderer _razorViewToStringRenderer)
{
    /// <summary>
    /// Builds email content asynchronously for the specified <typeparamref name="TData"/>.
    /// </summary>
    /// <typeparam name="TData">The type of data implementing <see cref="IEmailContent"/>.</typeparam>
    /// <param name="data">The data object implementing <see cref="IEmailContent"/>.</param>
    /// <returns>An instance of <see cref="BuiltEmailContent"/> containing the subject and body of the email.</returns>
    /// <exception cref="InvalidOperationException">Thrown when an invalid type of <see cref="IEmailContent"/> is provided.</exception>
    public async Task<BuiltEmailContent> BuildEmailContentAsync<TData>(TData data) where TData : IEmailContent
    {
        return data switch
        {
            ConfirmUserEmailEmailContent confirmUserEmailEmailContent => await BuildEmailContentAsync(confirmUserEmailEmailContent),
            SupportNotificationEmailContent supportNotificationEmailContent => await BuildEmailContentAsync(supportNotificationEmailContent),
            ResetPasswordEmailContent forgotPasswordEmailContent => await BuildEmailContentAsync(forgotPasswordEmailContent),
            _ => throw new InvalidOperationException("Invalid type of IEmail Content. Did you forget to register it inside the EmailContentBuilder?"),
        };
    }

    private async Task<BuiltEmailContent> BuildEmailContentAsync(ConfirmUserEmailEmailContent data)
    {
        var confirmUserModel = new ConfirmUserEmailViewModel(data.UserName, data.Token);
        string body = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ConfirmUserEmail/ConfirmUserEmail.cshtml", confirmUserModel);
        
        return new BuiltEmailContent("Confirm User Email", body);
    }
    
    private async Task<BuiltEmailContent> BuildEmailContentAsync(ResetPasswordEmailContent data)
    {
        var confirmUserModel = new ResetPasswordEmailViewModel(data.UserName, data.Token);
        string body = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ResetPasswordEmail/ResetPasswordEmail.cshtml", confirmUserModel);

        return new BuiltEmailContent("Reset Password", body);
    }

    private async Task<BuiltEmailContent> BuildEmailContentAsync(SupportNotificationEmailContent data)
    {
        var confirmUserModel = new SupportNotificationEmailViewModel(data.Message);
        string body = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/SupportNotificationEmail/SupportNotificationEmail.cshtml", confirmUserModel);

        return new BuiltEmailContent("Support Notification", body);
    }
}

public record BuiltEmailContent(string Subject, string Body);