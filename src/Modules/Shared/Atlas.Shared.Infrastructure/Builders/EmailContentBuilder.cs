using Atlas.Shared.Application.Abstractions.Services.EmailService;
using Atlas.Shared.Application.EmailContent;
using Atlas.Shared.Infrastructure.Razor.Views.Emails.ConfirmUserEmail;
using Atlas.Shared.Infrastructure.Razor.Views.Emails.ContactFormSubmittedEmail;
using Atlas.Shared.Infrastructure.Razor.Views.Emails.ResetPasswordEmail;
using Atlas.Shared.Infrastructure.Razor.Views.Emails.SupportNotificationEmail;
using Razor.Templating.Core;

namespace Atlas.Shared.Infrastructure.Builders;

/// <summary>
/// Builds email content asynchronously using Razor views for templating.
/// </summary>
public class EmailContentBuilder()
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
            ContactFormSubmittedEmailContent contactFormSubmittedEmailContent => await BuildEmailContentAsync(contactFormSubmittedEmailContent),
            _ => throw new InvalidOperationException("Invalid type of IEmail Content. Did you forget to register it inside the EmailContentBuilder?"),
        };
    }

    private async Task<BuiltEmailContent> BuildEmailContentAsync(ConfirmUserEmailEmailContent data)
    {
        var confirmUserModel = new ConfirmUserEmailViewModel(data.UserName, data.Token);
        string body = await RazorTemplateEngine.RenderAsync("/Views/Emails/ConfirmUserEmail/ConfirmUserEmail.cshtml", confirmUserModel);

        return new BuiltEmailContent("Confirm User Email", body);
    }
    
    private async Task<BuiltEmailContent> BuildEmailContentAsync(ResetPasswordEmailContent data)
    {
        var confirmUserModel = new ResetPasswordEmailViewModel(data.UserName, data.Token);
        string body = await RazorTemplateEngine.RenderAsync("/Views/Emails/ResetPasswordEmail/ResetPasswordEmail.cshtml", confirmUserModel);

        return new BuiltEmailContent("Reset Password", body);
    }

    private async Task<BuiltEmailContent> BuildEmailContentAsync(SupportNotificationEmailContent data)
    {
        var confirmUserModel = new SupportNotificationEmailViewModel(data.Message);
        string body = await RazorTemplateEngine.RenderAsync("/Views/Emails/SupportNotificationEmail/SupportNotificationEmail.cshtml", confirmUserModel);

        return new BuiltEmailContent("Support Notification", body);
    }

    private async Task<BuiltEmailContent> BuildEmailContentAsync(ContactFormSubmittedEmailContent data)
    {
        var confirmUserModel = new ContactFormSubmittedEmailViewModel(data.OccuredOnUtc, data.Name, data.Email, data.Message, data.Type);
        string body = await RazorTemplateEngine.RenderAsync("/Views/Emails/ContactFormSubmittedEmail/ContactFormSubmittedEmail.cshtml", confirmUserModel);

        return new BuiltEmailContent("Contact Form Submitted - " + data.Type, body);
    }
}

public record BuiltEmailContent(string Subject, string Body);