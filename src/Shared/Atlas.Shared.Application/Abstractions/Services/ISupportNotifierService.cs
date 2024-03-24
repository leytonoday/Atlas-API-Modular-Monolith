namespace Atlas.Shared.Application.Abstractions.Services;

/// <summary>
/// Represents a service that notifies the support team that maintain this API of some message. The notification mechanism could be email, SMS, etc. 
/// The message could be an exception, for example.
/// </summary>
public interface ISupportNotifierService
{
    /// <summary>
    /// Attempts to notify the support team with the given message. "Attempt" because the transport mechanism for the notification (email, for example) may be unavailable, and thus
    /// the support email will not send.
    /// </summary>
    /// <param name="message">The message to be sent to the support team.</param>
    /// <param name="cancellationToken">Propogates notifications that operations should be cancelled.</param>
    public Task AttemptNotifyAsync(string message, CancellationToken cancellationToken = default);
}
