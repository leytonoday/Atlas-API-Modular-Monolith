using MediatR;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Atlas.Plans.Domain;
using Atlas.Users.Application.Abstractions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Users.Domain.Errors;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.CreateSubscription;

internal sealed class CreateSubscriptionCommandHandler(IStripeCustomerRepository stripeCustomerRepository, IPlanRepository planRepository, IUserContext userContext, UserManager<User> userManager, IStripeService stripeService) : ICommandHandler<CreateSubscriptionCommand, Subscription>
{
    public async Task<Subscription> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        if (request.Interval != "month" && request.Interval != "year")
        {
            throw new ErrorException(PlansDomainErrors.Stripe.InvalidSubscriptionInterval);
        }

        User user = await userManager.FindByIdAsync(userContext.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(user.Id, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        // Ensure user is not already subscribed to a plan. We get the subscription, rather than checking the PlanId column because when the 
        // Subscription is past_due, the PlanId is revoked, but the subscription still exists.
        Subscription? existingSubscription = await stripeService.GetStripeCustomerSubscriptionAsync(stripeCustomer.StripeCustomerId, cancellationToken);
        if (existingSubscription is not null)
        {
            throw new ErrorException(PlansDomainErrors.Stripe.UserAlreadySubscribedToPlan);
        }

        var plan = await planRepository.GetByIdAsync(request.PlanId, false, cancellationToken)
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        // Get the promotion code, if one was specified, and ensure it is valid
        PromotionCode? promotionCode = null;
        if (request.PromotionCode is not null)
        {
            promotionCode = await stripeService.GetPromotionCodeFromPromotionCodeAsync(request.PromotionCode, cancellationToken);
            if (promotionCode is null)
            {
                throw new ErrorException(PlansDomainErrors.Stripe.InvalidPromotionCode);
            }
        }

        // Cancel any existing incomplete or incomplete_expired subscriptions.
        var subscriptionListOptions = new SubscriptionListOptions
        {
            Customer = stripeCustomer.StripeCustomerId
        };
        IEnumerable<Subscription> incompleteSubscriptions =
            (await stripeService.SubscriptionService.ListAsync(subscriptionListOptions, cancellationToken: cancellationToken))
            .Where(x => x.Status.Contains("incomplete"));
        foreach (Subscription incompleteSubscription in incompleteSubscriptions)
            await stripeService.SubscriptionService.CancelAsync(incompleteSubscription.Id, cancellationToken: cancellationToken);

        // Void all open invoices, just as a precaution.
        await stripeService.VoidAllOpenInvoicesAsync(stripeCustomer.StripeCustomerId, cancellationToken);

        // Get the target price, based on the specified interval (month or year)
        IEnumerable<Price> prices = await stripeService.GetProductPrices(plan.StripeProductId!, false, cancellationToken);
        Price? targetPrice = prices.FirstOrDefault(x => x.Recurring.Interval == request.Interval);

        // Get if the user is eligible for a trial
        bool isEligibleForTrial = await stripeService.IsUserEligibleForTrialAsync(stripeCustomer.StripeCustomerId, cancellationToken);

        // If the payment method provided has been used for a subscription before, the user is not eligible for a trial.
        if (request.StripePaymentMethodId is not null && await stripeService.HasCardPaymentMethodBeenUsedBeforeAsync(request.StripePaymentMethodId, cancellationToken))
            isEligibleForTrial = false;

        // Perform the Subscription creation
        var subscriptionCreateOptions = new SubscriptionCreateOptions
        {
            Customer = stripeCustomer.StripeCustomerId,
            Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Price = targetPrice!.Id,
                    }
                },
            PaymentSettings = new SubscriptionPaymentSettingsOptions()
            {
                PaymentMethodTypes = new List<string> { "card" },
                SaveDefaultPaymentMethod = "on_subscription",
            },
            DefaultPaymentMethod = request.StripePaymentMethodId,
            PaymentBehavior = "allow_incomplete",
            Expand = new List<string> { "latest_invoice.payment_intent", "pending_setup_intent" },
            Metadata = new Dictionary<string, string>
                {
                    { "planId", request.PlanId.ToString() }
                },
            TrialPeriodDays = isEligibleForTrial ? plan.TrialPeriodDays : 0,
            ProrationBehavior = "always_invoice",
            PromotionCode = promotionCode?.Id
        };

        Subscription? subscription = null;
        try
        {
            subscription = await stripeService.SubscriptionService.CreateAsync(subscriptionCreateOptions, cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            throw new ErrorException(PlansDomainErrors.Stripe.UnknownError(exception.Message));
        }

        return subscription;
    }
}
