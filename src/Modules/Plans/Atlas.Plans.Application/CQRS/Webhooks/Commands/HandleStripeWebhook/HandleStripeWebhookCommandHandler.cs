using Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Services;
using Atlas.Shared.Application.ModuleBridge;
using Atlas.Shared.Domain.Exceptions;
using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Webhooks.Commands.HandleStripeWebhook;

internal sealed class HandleStripeWebhookCommandHandler(
    IStripeService stripeService,
    IStripeCustomerRepository stripeCustomerRepository,
    IModuleBridge moduleBridge,
    ISupportNotifierService supportNotifierService,
    IStripeCardFingerprintRepository stripeCardFingerprintRepository) : IRequestHandler<HandleStripeWebhookCommand>
{
    public async Task Handle(HandleStripeWebhookCommand request, CancellationToken cancellationToken)
    {
        switch (request.StripeEvent.Type)
        {
            // User subscription has been cancelled, this event is fired at the end of the billing period
            case Events.CustomerSubscriptionDeleted:
            {
                await HandleCustomerSubscriptionDeletedAsync(request.StripeEvent, cancellationToken);
                break;
            }
            // User invoice failed at the end of the billing period
            case Events.InvoicePaymentFailed:
            {
                await HandleInvoicePaymentFailedAsync(request.StripeEvent, cancellationToken);
                break;
            }
            case Events.InvoicePaymentSucceeded: // TODO - Deprecated. Switch to InvoicePaid
            {
                await HandleInvoicePaymentSuceededAsync(request.StripeEvent,cancellationToken);
                break;
            }
            case Events.ChargeRefundUpdated:
            {
                await HandleChargeRefundUpdatedAsync(request.StripeEvent, cancellationToken);
                break;
            }
        }
    }

    /// <summary>
    /// Handles the event when a <see cref="Invoice"/> payment fails.
    /// </summary>
    /// <param name="webhookEvent">The event.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    private async Task HandleInvoicePaymentSuceededAsync(Event webhookEvent, CancellationToken cancellationToken)
    {
        // Get the invoice
        if (webhookEvent.Data.Object is not Invoice invoice)
            return;

        // Get the subscription and the plan Id from the subscription's metadata
        Subscription subscription = await stripeService.SubscriptionService.GetAsync(invoice.SubscriptionId, cancellationToken: cancellationToken);
        if (!subscription.Metadata.TryGetValue("planId", out string? planId))
            return;

        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByStripeCustomerId(subscription.CustomerId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        await AddCardFingerprintIfNewAsync(invoice, subscription, cancellationToken);

        // Set the user's planId to the planId that was specified in the subscription's metadata.
        // The result of this is that this user should have access to all of this Plan's features
        await moduleBridge.SetUserPlanId(stripeCustomer.UserId, new Guid(planId), cancellationToken);
    }

    /// <summary>
    /// Adds a Stripe <see cref="PaymentMethodCard"/>'s fingerprint value to the database if it isn't already in there.
    /// </summary>
    /// <param name="invoice">The <see cref="Invoice"/> of the successful payment.</param>
    /// <param name="subscription">The <see cref="Subscription"/> from which the invoice was taken.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    private async Task AddCardFingerprintIfNewAsync(Invoice invoice, Subscription subscription, CancellationToken cancellationToken)
    {
        string cardFingerprint;

        // If there was no payment intent, then this webhook will have come from a £0 invoice, which means it's a trial. 
        // Therefore, we need to get the payment method from the subscription, rather than any invoice.
        if (invoice.PaymentIntent is null)
        {
            PaymentMethod paymentMethod = await stripeService.PaymentMethodService.GetAsync(subscription.DefaultPaymentMethodId, cancellationToken: cancellationToken);
            cardFingerprint = paymentMethod.Card.Fingerprint;
        }
        // If there is a payment intent, then that means that this webhook comes from a non-£0 invoice, which means it's actually been paid.
        // Therefore, we can get the card from the payment method used for the invoice.
        else
        {
            PaymentIntent paymentIntent = await stripeService.PaymentIntentService.GetAsync(invoice.PaymentIntentId, cancellationToken: cancellationToken);
            PaymentMethod paymentMethod = await stripeService.PaymentMethodService.GetAsync(paymentIntent.PaymentMethodId, cancellationToken: cancellationToken);
            cardFingerprint = paymentMethod.Card.Fingerprint;
        }

        // The approach above means that a user can't use the same card for a trial, even if that card has been used to make regular non-trial payments.
        // For example, if another user has used that card at all, for any Atlas related payment, it is now ineligable for the trial.

        StripeCardFingerprint? existingCardFingerprint = await stripeCardFingerprintRepository.GetByFingerprintAsync(cardFingerprint, false, cancellationToken);

        // Fingerprint already added, so just continue
        if (existingCardFingerprint is not null)
            return;

        // Save the card fingerprint for future reference. Used later to check if the user is eligible for a trial.
        var cardFingerprintObject = StripeCardFingerprint.Create(cardFingerprint);

        await stripeCardFingerprintRepository.AddAsync(cardFingerprintObject, cancellationToken);
    }

    /// <summary>
    /// Handle the event when a <see cref="Invoice"/> payment fails.
    /// </summary>
    /// <param name="webhookEvent">The event.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    private async Task HandleInvoicePaymentFailedAsync(Event webhookEvent, CancellationToken cancellationToken)
    {
        // Get the invoice
        if (webhookEvent.Data.Object is not Invoice invoice)
            return;

        // Get the customer
        Customer customer = await stripeService.CustomerService.GetAsync(invoice.CustomerId, cancellationToken: cancellationToken);

        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByStripeCustomerId(customer.Id, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        // TODO - Send email to user, AND specify the reason for the failure.

        // Clear the user's planId. The result of clearing this planId means the user cannot access any of that Plan's features anymore
        await moduleBridge.SetUserPlanId(stripeCustomer.UserId, null, cancellationToken);
    }

    /// <summary>
    /// Handles the event when a <see cref="Customer"/>'s subscription is cancelled.
    /// </summary>
    /// <param name="webhookEvent">The event.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    private async Task HandleCustomerSubscriptionDeletedAsync(Event webhookEvent, CancellationToken cancellationToken)
    {
        // Get the subscription
        if (webhookEvent.Data.Object is not Subscription subscription)
            return;

        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByStripeCustomerId(subscription.CustomerId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        // Clear the user's planId
        await moduleBridge.SetUserPlanId(stripeCustomer.UserId, null, cancellationToken);

        // If the user's subscription is cancelled automatically as a result of multiple failed payments, then we need to void all open invoices.
        await stripeService.VoidAllOpenInvoicesAsync(stripeCustomer.StripeCustomerId, cancellationToken);
    }

    /// <summary>
    /// Handles the event when a <see cref="Stripe.Refund"/> fails. We must communicate this to both the support email, and the customer themselves.
    /// </summary>
    /// <param name="webhookEvent">The event.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    private async Task HandleChargeRefundUpdatedAsync(Event webhookEvent, CancellationToken cancellationToken)
    {
        if (webhookEvent.Data.Object is not Refund refund)
            return;

        if (refund.Status == "succeeded" || refund.Status == "pending" || refund.Status == "canceled")
            return;

        // Using the attached Charge, get the customer Id, and then using that, get the User
        Charge charge = await stripeService.ChargeService.GetAsync(refund.ChargeId, null, null, cancellationToken);

        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByStripeCustomerId(charge.CustomerId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        // Alert the user, and the support email of the refund failure
        await supportNotifierService.AttemptNotifyAsync(@$"Refund for {refund.Amount / 100.0} {refund.Currency.ToUpper()} failed for user with Id {stripeCustomer.UserId}. Reason: '{refund.FailureReason}'.", cancellationToken);

        // Before a refund is processed, we directly remove the credit from their account that downgrading would've given them. 
        // If the refund fails, then we need to add that credit back to their account.
        var customerBalanceTransactionCreateOptions = new CustomerBalanceTransactionCreateOptions
        {
            Amount = -refund.Amount,
            Currency = refund.Currency
        };
        await stripeService.CustomerBalanceTransactionService.CreateAsync(stripeCustomer.StripeCustomerId, customerBalanceTransactionCreateOptions, cancellationToken: cancellationToken);
    }

}
