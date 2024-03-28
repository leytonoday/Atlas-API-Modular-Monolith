using Atlas.Shared.Application.Abstractions.Services.EmailService;

namespace Atlas.Shared.Application.EmailContent;

/// <summary>
/// Represents the content of an email for notifying the support team of something.
/// </summary>
public sealed record SupportNotificationEmailContent(string Message) : IEmailContent;
