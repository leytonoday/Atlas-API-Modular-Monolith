using Atlas.Shared.Application.Abstractions.Services.EmailService;
using Atlas.Shared.Infrastructure.Builders;
using Atlas.Shared.Infrastructure.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Atlas.Shared.Infrastructure.Services;


/// <inheritdoc />
/// <param name="emailContentBuilder">The builder for constructing email content.</param>
/// <param name="emailOptions">The configuration options for the email service.</param>
public class EmailService(EmailContentBuilder emailContentBuilder, IOptions<EmailOptions> emailOptions) : IEmailService
{
    private EmailOptions _emailOptions => emailOptions.Value;

    /// <inheritdoc />
    public async Task SendEmailAsync(IEnumerable<string> recipients, IEmailContent emailContent, CancellationToken cancellationToken)
    {
        // Build Email
        MimeMessage builtEmail = await BuildEmail(recipients, emailContent);

        // Create SMTP client and send email
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailOptions.Host, _emailOptions.Port, _emailOptions.UseSsl, cancellationToken);

        // Only authenticate if we are in production
        if (!string.IsNullOrEmpty(_emailOptions.UserName) && !string.IsNullOrEmpty(_emailOptions.Password))
        {
            await smtp.AuthenticateAsync(_emailOptions.UserName, _emailOptions.Password, cancellationToken);
        }

        // Send, then disconnect
        await smtp.SendAsync(builtEmail, cancellationToken);
        await smtp.DisconnectAsync(true, cancellationToken);
    }

    /// <inheritdoc />
    public async Task SendEmailAsync(string recipient, IEmailContent emailContent, CancellationToken cancellationToken)
    {
        await SendEmailAsync(new List<string>() { recipient }, emailContent, cancellationToken);
    }

    /// <summary>
    /// Constructs a MimeMessage for the specified recipients and email content.
    /// </summary>
    /// <param name="recipients">A collection of email addresses representing the recipients of the email.</param>
    /// <param name="emailContent">The content of the email to be built.</param>
    /// <returns>A MimeMessage representing the constructed email.</returns>
    /// <remarks>
    /// This method builds an email with the specified recipients and content, utilizing the configured
    /// SMTP settings and email options. The sender's display name and credentials, if available, are
    /// incorporated into the MimeMessage.
    /// </remarks>
    private async Task<MimeMessage> BuildEmail(IEnumerable<string> recipients, IEmailContent emailContent)
    {
        BuiltEmailContent result = await emailContentBuilder.BuildEmailContentAsync(emailContent);

        var email = new MimeMessage();

        // Add the sender, and the sender's display name 
        email.From.Add(MailboxAddress.Parse($"{_emailOptions.DisplayName} <{_emailOptions.UserName}>"));

        // Add the recipients
        foreach (string recipient in recipients)
        {
            email.To.Add(MailboxAddress.Parse(recipient));
        }

        email.Subject = result.Subject;

        var bodyBuilder = new BodyBuilder()
        {
            HtmlBody = result.Body
        };

        email.Body = bodyBuilder.ToMessageBody();

        return email;
    }
}
