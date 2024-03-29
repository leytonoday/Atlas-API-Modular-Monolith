﻿using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Abstractions.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Webhooks.Commands.HandleStripeWebhook;

internal sealed class HandleStripeWebhookCommandHandler: IRequestHandler<HandleStripeWebhookCommand>
{
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IStripeService _stripeService;

    public HandleStripeWebhookCommandHandler(IStripeService stripeService, IBackgroundTaskQueue backgroundTaskQueue, IServiceScopeFactory serviceScopeFactory)
    {
        _backgroundTaskQueue = backgroundTaskQueue;
        _serviceScopeFactory = serviceScopeFactory;
        _stripeService = stripeService;
    }

    public async Task Handle(HandleStripeWebhookCommand request, CancellationToken cancellationToken)
    {
        await _backgroundTaskQueue.EnqueueAsync(async (cancellationToken) =>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var stripeService = scope.ServiceProvider.GetService<IStripeService>();
            var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
            var plansUnitOfWork = scope.ServiceProvider.GetService<IPlansUnitOfWork>();
            var supportNotifierService = scope.ServiceProvider.GetService<ISupportNotifierService>();

            if (stripeService is null ||  userManager is null || plansUnitOfWork is null || supportNotifierService is null)
            {
                throw new Exception("Could not resolve services");
            }

            switch (request.StripeEvent.Type)
            {
                // User subscription has been cancelled, this event is fired at the end of the billing period
                case Events.CustomerSubscriptionDeleted:
                {
                    await HandleCustomerSubscriptionDeletedAsync(plansUnitOfWork, userManager, stripeService, request.StripeEvent, cancellationToken);
                    break;
                }
                // User invoice failed at the end of the billing period
                case Events.InvoicePaymentFailed:
                {
                    await HandleInvoicePaymentFailedAsync(plansUnitOfWork, userManager, request.StripeEvent, cancellationToken);
                    break;
                }
                case Events.InvoicePaymentSucceeded: // TODO - Deprecated. Switch to InvoicePaid
                {
                    await HandleInvoicePaymentSuceededAsync(
                        request.StripeEvent,
                        stripeService, 
                        userManager, 
                        plansUnitOfWork,
                        cancellationToken);
                    break;
                }
                case Events.ChargeRefundUpdated:
                {
                    await HandleChargeRefundUpdatedAsync(plansUnitOfWork, userManager, supportNotifierService, request.StripeEvent, cancellationToken);
                    break;
                }
            }
        });
    }

    /// <summary>
    /// Handles the event when a <see cref="Invoice"/> payment fails.
    /// </summary>
    /// <param name="webhookEvent">The event.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    private async Task HandleInvoicePaymentSuceededAsync(Event webhookEvent, IStripeService stripeService, UserManager<User> userManager, IPlansUnitOfWork plansUnitOfWork, CancellationToken cancellationToken)
    {
        // Get the invoice
        if (webhookEvent.Data.Object is not Invoice invoice)
            return;

        // Get the subscription and the plan Id from the subscription's metadata
        Subscription subscription = await stripeService.SubscriptionService.GetAsync(invoice.SubscriptionId, cancellationToken: cancellationToken);
        if (!subscription.Metadata.TryGetValue("planId", out string? planId))
            return;

        StripeCustomer? stripeCustomer = await plansUnitOfWork.StripeCustomerRepository.GetByStripeCustomerId(subscription.CustomerId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        User user = await userManager.FindByIdAsync(stripeCustomer.UserId.ToString())
          ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        // Set the user's planId to the planId that was specified in the subscription's metadata
        User.SetPlanId(user, new Guid(planId));

        await AddCardFingerprintIfNewAsync(plansUnitOfWork, invoice, subscription, cancellationToken);

        await userManager.UpdateAsync(user);
        await plansUnitOfWork.CommitAsync(cancellationToken);
    }

    /// <summary>
    /// Adds a Stripe <see cref="PaymentMethodCard"/>'s fingerprint value to the database if it isn't already in there.
    /// </summary>
    /// <param name="invoice">The <see cref="Invoice"/> of the successful payment.</param>
    /// <param name="subscription">The <see cref="Subscription"/> from which the invoice was taken.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    private async Task AddCardFingerprintIfNewAsync(IPlansUnitOfWork plansUnitOfWork, Invoice invoice, Subscription subscription, CancellationToken cancellationToken)
    {
        string cardFingerprint;

        // If there was no payment intent, then this webhook will have come from a £0 invoice, which means it's a trial. 
        // Therefore, we need to get the payment method from the subscription, rather than any invoice.
        if (invoice.PaymentIntent is null)
        {
            PaymentMethod paymentMethod = await _stripeService.PaymentMethodService.GetAsync(subscription.DefaultPaymentMethodId, cancellationToken: cancellationToken);
            cardFingerprint = paymentMethod.Card.Fingerprint;
        }
        // If there is a payment intent, then that means that this webhook comes from a non-£0 invoice, which means it's actually been paid.
        // Therefore, we can get the card from the payment method used for the invoice.
        else
        {
            PaymentIntent paymentIntent = await _stripeService.PaymentIntentService.GetAsync(invoice.PaymentIntentId, cancellationToken: cancellationToken);
            PaymentMethod paymentMethod = await _stripeService.PaymentMethodService.GetAsync(paymentIntent.PaymentMethodId, cancellationToken: cancellationToken);
            cardFingerprint = paymentMethod.Card.Fingerprint;
        }

        // The approach above means that a user can't use the same card for a trial, even if that card has been used to make regular non-trial payments.
        // For example, if another user has used that card at all, for any Clipboard TTS related payment, it is now ineligable for the trial.

        StripeCardFingerprint? existingCardFingerprint = await plansUnitOfWork.StripeCardFingerprintRepository.GetByFingerprintAsync(cardFingerprint, false, cancellationToken);

        // Fingerprint already added, so just continue
        if (existingCardFingerprint is not null)
            return;

        // Save the card fingerprint for future reference. Used later to check if the user is eligible for a trial.
        var cardFingerprintObject = StripeCardFingerprint.Create(cardFingerprint);

        await plansUnitOfWork.StripeCardFingerprintRepository.AddAsync(cardFingerprintObject, cancellationToken);
    }

    /// <summary>
    /// Handle the event when a <see cref="Invoice"/> payment fails.
    /// </summary>
    /// <param name="webhookEvent">The event.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    private async Task HandleInvoicePaymentFailedAsync(IPlansUnitOfWork plansUnitOfWork, UserManager<User> userManager, Event webhookEvent, CancellationToken cancellationToken)
    {
        // Get the invoice
        if (webhookEvent.Data.Object is not Invoice invoice)
            return;

        // Get the customer
        Customer customer = await _stripeService.CustomerService.GetAsync(invoice.CustomerId, cancellationToken: cancellationToken);

        StripeCustomer? stripeCustomer = await plansUnitOfWork.StripeCustomerRepository.GetByStripeCustomerId(customer.Id, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        User user = await userManager.FindByIdAsync(stripeCustomer.UserId.ToString())
          ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        // TODO - Send email to user, AND specify the reason for the failure.

        // Clear the user's planId
        User.SetPlanId(user, null);

        await userManager.UpdateAsync(user);
        await plansUnitOfWork.CommitAsync(cancellationToken);
    }

    /// <summary>
    /// Handles the event when a <see cref="Customer"/>'s subscription is cancelled.
    /// </summary>
    /// <param name="webhookEvent">The event.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    private async Task HandleCustomerSubscriptionDeletedAsync(IPlansUnitOfWork plansUnitOfWork, UserManager<User> userManager, IStripeService stripeService, Event webhookEvent, CancellationToken cancellationToken)
    {
        // Get the subscription
        if (webhookEvent.Data.Object is not Subscription subscription)
            return;

        StripeCustomer? stripeCustomer = await plansUnitOfWork.StripeCustomerRepository.GetByStripeCustomerId(subscription.CustomerId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        User user = await userManager.FindByIdAsync(stripeCustomer.UserId.ToString())
          ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        // Clear the user's planId
        User.SetPlanId(user, null);

        await userManager.UpdateAsync(user);
        await plansUnitOfWork.CommitAsync(cancellationToken);

        // If the user's subscription is cancelled automatically as a result of multiple failed payments, then we need to void all open invoices.
        await stripeService.VoidAllOpenInvoicesAsync(stripeCustomer.StripeCustomerId, cancellationToken);
    }

        /// <summary>
    /// Handles the event when a <see cref="Stripe.Refund"/> fails. We must communicate this to both the support email, and the customer themselves.
    /// </summary>
    /// <param name="webhookEvent">The event.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    private async Task HandleChargeRefundUpdatedAsync(IPlansUnitOfWork plansUnitOfWork, UserManager<User> userManager, ISupportNotifierService supportNotifierService, Event webhookEvent, CancellationToken cancellationToken)
    {
        if (webhookEvent.Data.Object is not Refund refund)
            return;

        // Using the attached Charge, get the customer Id, and then using that, get the User
        Charge charge = await _stripeService.ChargeService.GetAsync(refund.ChargeId, null, null, cancellationToken);

        StripeCustomer? stripeCustomer = await plansUnitOfWork.StripeCustomerRepository.GetByStripeCustomerId(charge.CustomerId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        User user = await userManager.FindByIdAsync(stripeCustomer.UserId.ToString())
          ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        if (refund.Status == "succeeded" || refund.Status == "pending" || refund.Status == "canceled")
            return;

        //// Alert the user, and the support email of the refund failure
        await supportNotifierService.AttemptNotifyAsync(@$"Refund for {refund.Amount / 100.0} {refund.Currency.ToUpper()} failed for user with Id {user.Id}. Reason: '{refund.FailureReason}'.", cancellationToken);

        // Before a refund is processed, we directly remove the credit from their account that downgrading would've given them. 
        // If the refund fails, then we need to add that credit back to their account.
        var customerBalanceTransactionCreateOptions = new CustomerBalanceTransactionCreateOptions
        {
            Amount = -refund.Amount,
            Currency = refund.Currency
        };
        await _stripeService.CustomerBalanceTransactionService.CreateAsync(stripeCustomer.StripeCustomerId, customerBalanceTransactionCreateOptions, cancellationToken: cancellationToken);
    }

}