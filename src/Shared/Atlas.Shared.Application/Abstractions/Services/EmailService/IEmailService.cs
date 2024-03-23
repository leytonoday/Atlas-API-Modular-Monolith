namespace Atlas.Shared.Application.Abstractions.Services.EmailService;

/// <summary>
/// Represents a service for sending emails.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email to multiple recipients listed in <paramref name="recipients"/>.
    /// </summary>
    /// <param name="recipients">A list of emails addressed to which the email should be sent.</param>
    /// <param name="emailContent">A marker interface to indicate that the contents of this object are data for an email.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancellled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    public Task SendEmailAsync(IEnumerable<string> recipients, IEmailContent emailContent, CancellationToken cancellationToken);

    /// <summary>
    /// Sends an email to one <paramref name="recipient"/>.
    /// </summary>
    /// <param name="recipient">The single email address to which the email should be sent.</param>
    /// <param name="emailContent">A marker interface to indicate that the contents of this object are data for an email.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancellled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    public Task SendEmailAsync(string recipient, IEmailContent emailContent, CancellationToken cancellationToken);
}
