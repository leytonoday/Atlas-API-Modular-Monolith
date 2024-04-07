using Atlas.Plans.Application.CQRS.Stripe.Commands.AttachPaymentMethod;
using Atlas.Plans.Application.CQRS.Stripe.Commands.CancelSubscription;
using Atlas.Plans.Application.CQRS.Stripe.Commands.CreateSubscription;
using Atlas.Plans.Application.CQRS.Stripe.Commands.ReactivateSubscription;
using Atlas.Plans.Application.CQRS.Stripe.Commands.SetSubscriptionPaymentMethod;
using Atlas.Plans.Application.CQRS.Stripe.Commands.UpdateSubscription;
using Atlas.Plans.Application.CQRS.Stripe.Queries.GetCustomerPortalLink;
using Atlas.Plans.Application.CQRS.Stripe.Queries.GetHasPaymentMethodBeenUsedBefore;
using Atlas.Plans.Application.CQRS.Stripe.Queries.GetInvoiceHistory;
using Atlas.Plans.Application.CQRS.Stripe.Queries.GetIsUserEligibleForTrial;
using Atlas.Plans.Application.CQRS.Stripe.Queries.GetPublishableKey;
using Atlas.Plans.Application.CQRS.Stripe.Queries.GetSubscriptionQuoteInvoice;
using Atlas.Plans.Application.CQRS.Stripe.Queries.GetUpcomingInvoice;
using Atlas.Plans.Application.CQRS.Stripe.Queries.GetUserDefaultPaymentMethod;
using Atlas.Plans.Application.CQRS.Stripe.Queries.GetUserPaymentMethodsQuery;
using Atlas.Plans.Application.CQRS.Stripe.Queries.GetUserSubscription;
using Atlas.Plans.Infrastructure.Module;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Domain.Results;
using Atlas.Web.Modules.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.Web.Modules.Plans.Controllers;

[Authorize]
[Route("/api/{version:apiVersion}/stripe")]
[ApiVersion("1.0")]
public class StripeController(IExecutionContextAccessor executionContext, IPlansModule plansModule) : ApiController
{
    [HttpGet("payment-methods/default")]
    public async Task<IActionResult> GetMyDefaultPaymentMethod(CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(new GetUserDefaultPaymentMethodQuery(executionContext.UserId), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpGet("payment-methods")]
    public async Task<IActionResult> GetMyPaymentMethods(CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(new GetUserPaymentMethodsQuery(executionContext.UserId), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpGet("has-card-payment-method-been-used-before/{paymentMethodId}")]
    public async Task<IActionResult> HasCardPaymentBeenUsedBefore([FromRoute] string paymentMethodId, CancellationToken cancellationToken)
    {
        await plansModule.SendQuery(new GetHasPaymentMethodBeenUsedBeforeQuery(paymentMethodId), cancellationToken);
        return Ok(Result.Success());
    }

    [HttpPost("payment-methods/attach")]
    public async Task<IActionResult> AttachPaymentMethod([FromBody] AttachPaymentMethodCommand attachPaymentMethodCommand, CancellationToken cancellationToken)
    {
        await plansModule.SendCommand(attachPaymentMethodCommand, cancellationToken);
        return Ok(Result.Success());
    }

    [HttpPut("my-subscription/payment-method/{paymentMethodId}")]
    public async Task<IActionResult> SetSubscriptionPaymentMethod([FromRoute] string paymentMethodId, CancellationToken cancellationToken)
    {
        await plansModule.SendCommand(new SetSubscriptionPaymentMethodCommand(paymentMethodId, executionContext.UserId), cancellationToken);
        return Ok(Result.Success());
    }

    [HttpGet("my-subscription/upcoming-invoice")]
    public async Task<IActionResult> GetMyUpcomingInvoive(CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(new GetUpcomingInvoiceQuery(executionContext.UserId), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpGet("my-subscription")]
    public async Task<IActionResult> GetMySubscription(CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(new GetUserSubscriptionQuery(executionContext.UserId), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpPost("my-subscription/cancel")]
    public async Task<IActionResult> CancelMySubscription(CancellationToken cancellationToken)
    {
        await plansModule.SendCommand(new CancelSubscriptionCommand(executionContext.UserId), cancellationToken);
        return Ok(Result.Success());
    }

    [HttpPost("my-subscription/reactivate")]
    public async Task<IActionResult> ReactivateMySubscription(CancellationToken cancellationToken)
    {
        await plansModule.SendCommand(new ReactivateSubscriptionCommand(executionContext.UserId), cancellationToken);
        return Ok(Result.Success());
    }

    [HttpGet("my-subscription/invoice-history")]
    public async Task<IActionResult> GetMyInvoiceHistory([FromQuery] int? limit, [FromQuery] string? startingAfter, CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(new GetInvoiceHistoryQuery(executionContext.UserId, limit, startingAfter), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpGet("publishable-key")]
    public async Task<IActionResult> GetStripePublishableKey(CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(new GetPublishableKeyQuery(), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpPost("quote-invoice")]
    public async Task<IActionResult> GetSubscriptionQuoteInvoice([FromBody] GetSubscriptionQuoteInvoiceQuery request, CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(request, cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpPost("subscription")]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var result = await plansModule.SendCommand(request, cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpPut("subscription")]
    public async Task<IActionResult> UpdateSubscription([FromBody] UpdateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var result = await plansModule.SendCommand(request, cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpGet("customer-portal-link")]
    public async Task<IActionResult> GetCustomerPortalLink(CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(new GetCustomerPortalLinkQuery(executionContext.UserId), cancellationToken);
        return Ok(Result.Success(result));
    }


    [HttpGet("trial/eligible")]
    public async Task<IActionResult> GetAmIEligableForTrial(CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(new GetIsUserEligibleForTrialQuery(executionContext.UserId), cancellationToken);
        return Ok(Result.Success(result));
    }
}
