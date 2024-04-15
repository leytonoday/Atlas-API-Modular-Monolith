using Atlas.Shared.Application.Abstractions.Services.EmailService;
using Atlas.Shared.Application.EmailContent;
using Atlas.Web.Modules.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.Web.Controllers;

[Route("/api/{version:apiVersion}/contact")]
[ApiVersion("1.0")]
public class ContactController(IEmailService emailService) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> SendContactEmail(ContactEmailDto contactEmailDto, CancellationToken cancellationToken)
    {
        await emailService.SendEmailAsync("support@legallighthouse.xyz", new ContactFormSubmittedEmailContent(DateTime.UtcNow, contactEmailDto.Name, contactEmailDto.Email, contactEmailDto.Message, contactEmailDto.Type), cancellationToken);
        return Ok();
    }
}

public record ContactEmailDto(string Email, string Name, string Type, string Message);