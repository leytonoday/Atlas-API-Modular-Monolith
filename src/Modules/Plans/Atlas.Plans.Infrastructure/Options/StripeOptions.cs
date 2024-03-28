namespace Atlas.Plans.Infrastructure.Options;

/// <summary>
/// Represents a set of credentials for interacting with the Stripe API, which are loaded from the appsettings.json file.
/// </summary>
public class StripeOptions
{
    /// <summary>
    /// Gets or sets the publishable key for the Stripe API.
    /// </summary>
    public string PublishableKey { get; set; } = null!;

    /// <summary>
    /// Gets or sets the secret key for the Stripe API.
    /// </summary>
    public string SecretKey { get; set; } = null!;

    /// <summary>
    /// Gets or sets the webhook secret for the Stripe API, used to verify the contents of a webhook event.
    /// </summary>
    public string WebhookSecret { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Id of the current Test Clock for the Stripe API.
    /// </summary>
    public string? TestClockId { get; set; }
}
