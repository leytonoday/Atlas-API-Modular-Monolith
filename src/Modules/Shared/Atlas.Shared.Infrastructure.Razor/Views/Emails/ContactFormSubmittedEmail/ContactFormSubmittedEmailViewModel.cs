namespace Atlas.Shared.Infrastructure.Razor.Views.Emails.ContactFormSubmittedEmail;

public sealed record ContactFormSubmittedEmailViewModel(DateTime OccuredOnUtc, string Name, string Email, string Message, string Type, string Company);
