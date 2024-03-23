using Atlas.Shared.Application.Abstractions.Services.EmailService;

namespace Atlas.Users.Application.EmailContent;

/// <summary>
/// Represents the content of an email for confirming a user's email address.
/// </summary>
public record ConfirmUserEmailEmailContent(string UserName, string Token) : IEmailContent;