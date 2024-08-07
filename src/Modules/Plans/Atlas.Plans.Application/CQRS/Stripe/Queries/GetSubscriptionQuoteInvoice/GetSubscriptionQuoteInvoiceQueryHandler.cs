﻿using Stripe;
using Atlas.Plans.Application.CQRS.Stripe.Queries.GetSubscriptionQuoteInvoice;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Application.Abstractions;

namespace Atlas.Infrastructure.CQRS.Queries.GetInvoiceHistory;

internal sealed class GetSubscriptionQuoteInvoiceQueryHandler(IStripeCustomerRepository stripeCustomerRepository, IPlanRepository planRepository, IStripeService stripeService, IExecutionContextAccessor executionContext) : IQueryHandler<GetSubscriptionQuoteInvoiceQuery, Invoice>
{
    public async Task<Invoice> Handle(GetSubscriptionQuoteInvoiceQuery request, CancellationToken cancellationToken)
    {
        var plan = await planRepository.GetByIdAsync(request.PlanId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(executionContext.UserId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        Subscription? currentSubscription = await stripeService.GetStripeCustomerSubscriptionAsync(stripeCustomer.StripeCustomerId!, cancellationToken);

        // Get the target price, based on the specified interval (month or year)
        IEnumerable<Price> prices = await stripeService.GetProductPrices(plan.StripeProductId!, false, cancellationToken);
        Price targetPrice = prices.FirstOrDefault(x => x.Recurring.Interval == request.Interval)!;

        var upcomingInvoiceOptions = new UpcomingInvoiceOptions
        {
            Customer = stripeCustomer.StripeCustomerId,
            Subscription = currentSubscription?.Id, // Id of the user's current subscription. Null if there isn't one
            SubscriptionItems = new List<InvoiceSubscriptionItemOptions>
            {
                new InvoiceSubscriptionItemOptions
                {
                    Price = targetPrice.Id, // Price of the new item to subscribe to
                    Id = currentSubscription?.Items.FirstOrDefault()?.Id, // Id of current subscribed item. Should only ever be one item anyway
                }
            },
            SubscriptionProrationBehavior = "always_invoice", // Generates prorations
        };

        // Apply the coupon that the promotion code corresponds to, if provided
        if (!string.IsNullOrEmpty(request.PromotionCode))
        {
            Coupon? coupon = await stripeService.GetCouponFromPromotionCodeAsync(request.PromotionCode, cancellationToken);
            if (coupon is not null)
            {
                upcomingInvoiceOptions.Coupon = coupon.Id;
            }
        }
        else
        {
            // In the case where there isn't any promotion code specified, we're going to presume this means that no discount should be applied.
            // Specify an empty array to prevent it from inheriting discounts from the subscription
            upcomingInvoiceOptions.Discounts = new List<InvoiceDiscountOptions> { };
        }

        Invoice upcomingInvoice = await stripeService.InvoiceService.UpcomingAsync(upcomingInvoiceOptions, cancellationToken: cancellationToken);

        return upcomingInvoice;
    }
}
