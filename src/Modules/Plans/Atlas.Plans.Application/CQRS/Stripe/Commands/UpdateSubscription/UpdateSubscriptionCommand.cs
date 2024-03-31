using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.UpdateSubscription;

/// <summary>
/// Represents the data required to create a Stripe subscription
/// </summary>
/// <param name="StripePaymentMethodId">The ID of the Stripe Payment Method to use for the Subscription. This is set in the scenario where the user selects a pre-existing payment method to use.</param>
/// <param name="Interval">The length of the billing cycle. Either <c>month</c> or <c>year</c>.</param>
/// <param name="PlanId">The ID of the <see cref="Plan"/> to assign to the user, once the Stripe Invoice has been processed successfully.</param>
/// <param name="PromotionCode">A stripe promotion code that corresponds to a <see cref="Stripe.Coupon"/> that will be applied to the Subscription.</param>
public sealed record UpdateSubscriptionCommand(string? StripePaymentMethodId, string Interval, Guid PlanId, string? PromotionCode) : ICommand<Subscription>;