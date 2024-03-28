using Atlas.Shared.Application.Abstractions.Services.EmailService;
using Atlas.Shared.Application.Abstractions.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly.Retry;
using Polly;
using Atlas.Shared.Infrastructure.Options;
using Atlas.Shared.Application.EmailContent;

namespace Atlas.Shared.Infrastructure.Services;

/// <summary>
/// Represents a service used to send Email notifications to the support team that maintain this API
/// </summary>
public sealed class SupportNotifierService(IEmailService emailService, IOptions<SupportNotificationOptions> supportNotificationOptions, ILogger<SupportNotifierService> logger) : ISupportNotifierService
{
    private readonly static AsyncRetryPolicy _retryPolicy = Policy
    .Handle<Exception>()
    .WaitAndRetryAsync(
        3,
        attempt => TimeSpan.FromSeconds(10 * attempt)
    );

    /// <inheritdoc />
    public async Task AttemptNotifyAsync(string message, CancellationToken cancellationToken = default)
    {
        // Retry sending the email a few times if it doesn't work
        PolicyResult result = await _retryPolicy.ExecuteAndCaptureAsync(() =>
            emailService.SendEmailAsync(supportNotificationOptions.Value.Emails, new SupportNotificationEmailContent(message), cancellationToken));

        if (result.Outcome == OutcomeType.Failure)
        {
            logger.LogError("Could not send Support Notification. Error {error}", result.FinalException?.ToString() ?? "Unknown Error");
        }
    }
}
