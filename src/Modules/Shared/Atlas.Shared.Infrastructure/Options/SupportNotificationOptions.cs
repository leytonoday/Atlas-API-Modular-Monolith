namespace Atlas.Shared.Infrastructure.Options;

/// <summary>
/// Represents the options for notifying support of internal messages or errors
/// </summary>
public class SupportNotificationOptions
{
    /// <summary>
    /// Gets or sets a list of emails to which support notifications should be sent.
    /// </summary>
    public IEnumerable<string> Emails { get; set; } = Enumerable.Empty<string>();
}
