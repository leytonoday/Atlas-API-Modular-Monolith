using MediatR;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Atlas.Plans.Domain;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Abstractions;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.UpdateSubscription;

internal sealed class UpdateSubscriptionCommandHandler(IStripeCustomerRepository stripeCustomerRepository, IPlanRepository planRepository, IExecutionContextAccessor executionContext, IStripeService stripeService) : ICommandHandler<UpdateSubscriptionCommand, Subscription>
{
    public async Task<Subscription> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        if (request.Interval != "month" && request.Interval != "year")
        {
            throw new ErrorException(PlansDomainErrors.Stripe.InvalidSubscriptionInterval);
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

        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(executionContext.UserId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        // Cancel any existing incomplete or incomplete_expired subscriptions. These can occur if the user fails or abandons 3D Secure authentication
        var subscriptionListOptions = new SubscriptionListOptions
        {
            Customer = stripeCustomer.StripeCustomerId
        };
        IEnumerable<Subscription> incompleteSubscriptions =
            (await stripeService.SubscriptionService.ListAsync(subscriptionListOptions, cancellationToken: cancellationToken))
            .Where(x => x.Status.Contains("incomplete"));
        foreach (Subscription incompleteSubscription in incompleteSubscriptions)
            await stripeService.SubscriptionService.CancelAsync(incompleteSubscription.Id, cancellationToken: cancellationToken);

        // Get the user's current subscription
        Subscription currentSubscription = await stripeService.GetStripeCustomerSubscriptionAsync(stripeCustomer.StripeCustomerId, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Stripe.UserDoesNotHaveSubscription);

        // Cannot change subscription if it is past due (user must cancel current subscription and then subscribe to a new plan if they haven't paid. Makes things a little easier)
        if (currentSubscription.Status == SubscriptionStatuses.PastDue)
        {
            throw new ErrorException(PlansDomainErrors.Stripe.UserHasPastDueSubscription);
        }

        // Get the target price, based on the specified interval (month or year)
        IEnumerable<Price> prices = await stripeService.GetProductPrices(plan.StripeProductId!, false, cancellationToken);
        Price? targetPrice = prices.FirstOrDefault(x => x.Recurring.Interval == request.Interval);

        var subscriptionUpdateOptions = new SubscriptionUpdateOptions
        {
            Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Id = currentSubscription!.Items.Data.First().Id,
                        Price = targetPrice!.Id,
                    }
                },
            DefaultPaymentMethod = request.StripePaymentMethodId,
            PaymentBehavior = "allow_incomplete",
            // If there is no positive TrialPeriodDays value on the Subscription, then the payment is to be taken immediately, and thus an invoice with a payment intent is created.
            // If there is a positive TrialPeriodDays value on the Subscription, then the payment is to be taken at the end of the trial, and thus a setup intent is created instead.
            Expand = new List<string> { "latest_invoice.payment_intent", "pending_setup_intent" },
            ProrationBehavior = "always_invoice", // If the user is upgrading, this will cause an immediate pro-ration, charging the user the difference between the old and new plan.
            Metadata = new Dictionary<string, string>
                {
                    { "planId", request.PlanId.ToString() }
                },
            // If you've had a subscription previously, you aren't eligible for a trial. So when updating a subscription, we'll end the trial immediately, if there is one.
            TrialEnd = SubscriptionTrialEnd.Now,
            PromotionCode = promotionCode?.Id
        };

        Subscription? updatedSubscription = null;
        try
        {
            updatedSubscription = await stripeService.SubscriptionService.UpdateAsync(currentSubscription.Id, subscriptionUpdateOptions, cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            throw new ErrorException(PlansDomainErrors.Stripe.UnknownError(exception.Message));
        }

        // If the promo code has been removed, then remove the discount from the user's Stripe customer
        if (request.PromotionCode is null && updatedSubscription.Discount is not null)
            stripeService.DiscountService.DeleteSubscriptionDiscount(updatedSubscription.Id);

        await stripeService.ProcessRefundIfNecessaryAsync(updatedSubscription, stripeCustomer.StripeCustomerId, cancellationToken);

        return updatedSubscription;
    }
}
