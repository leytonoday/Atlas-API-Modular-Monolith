using Atlas.Plans.Application.CQRS.Webhooks.Commands.HandleStripeWebhook;
using Atlas.Plans.Infrastructure.Options;
using Atlas.Plans.Module;
using Atlas.Shared.Presentation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace Atlas.Web.Modules.Plans.Controllers;

[Route("/api/{version:apiVersion}/webhook")]
[ApiVersion("1.0")]
public class WebhookController(IOptions<StripeOptions> stripeOptions, IPlansModule plansModule) : ApiController
{
    private readonly StripeOptions _stripeOptions = stripeOptions.Value;

    [HttpPost("stripe")]
    public async Task<IActionResult> HandleStripeWebhook(CancellationToken cancellationToken)
    {
        string body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(cancellationToken);
        string? signature = Request.Headers["Stripe-Signature"];

        if (string.IsNullOrEmpty(signature))
            return BadRequest();

        Event? stripeEvent = EventUtility.ConstructEvent(body, signature, _stripeOptions.WebhookSecret);
        if (stripeEvent is null)
            return BadRequest();

        await plansModule.SendCommand(new HandleStripeWebhookCommand(stripeEvent), cancellationToken);

        return Ok();
    }
}

