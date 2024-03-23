using Atlas.Shared.Application.Abstractions.Services.EmailService;

namespace Atlas.Users.Application.EmailContent;

/// <summary>
/// Represents the content of an email for resetting a user's forgotten password.
/// </summary>
public record ResetPasswordEmailContent(string UserName, string Token) : IEmailContent;