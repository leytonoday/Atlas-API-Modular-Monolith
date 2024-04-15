using Atlas.Shared.Application.Abstractions.Services.EmailService;


namespace Atlas.Shared.Application.EmailContent;

public record ContactFormSubmittedEmailContent(DateTime OccuredOnUtc, string Name, string Email, string Message, string Type) : IEmailContent;
