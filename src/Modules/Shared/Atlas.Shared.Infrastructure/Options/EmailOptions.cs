namespace Atlas.Shared.Infrastructure.Options;

/// <summary>
/// Represents the options for sending emails, which are loaded from the appsettings.json file.
/// </summary>
public class EmailOptions
{
    /// <summary>
    /// Gets or sets the username for the email account used to send emails.
    /// </summary>
    public string UserName { get; set; } = "john.doe@example.com";

    /// <summary>
    /// Gets or sets the password for the email account used to send emails.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the display name to use when sending emails. When in email clients, this is the name which will be displayed as the sender.
    /// </summary>
    public string DisplayName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the SMTP server address to send emails.
    /// </summary>
    public string Host { get; set; } = null!;

    /// <summary>
    /// Gets or sets the SMTP server port used to send emails.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Determines if SSL should be used when sending any emails.
    /// </summary>
    public bool UseSsl { get; set; }
}
