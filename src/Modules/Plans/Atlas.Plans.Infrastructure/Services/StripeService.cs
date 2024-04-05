using Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Shared;
using Microsoft.Extensions.Options;
using Stripe;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Plans.Infrastructure.Options;

namespace Atlas.Plans.Infrastructure.Services;

using AtlasPlan = Atlas.Plans.Domain.Entities.PlanEntity.Plan;

public sealed class StripeService : IStripeService
{
    public ProductService StripeProductService { get; } = new ProductService();
    public PriceService StripePriceService { get; } = new PriceService();
    public Stripe.CustomerService CustomerService { get; } = new Stripe.CustomerService();
    public SubscriptionService SubscriptionService { get; } = new SubscriptionService();
    public PaymentMethodService PaymentMethodService { get; } = new PaymentMethodService();
    public Stripe.BillingPortal.SessionService BillingSessionService { get; } = new Stripe.BillingPortal.SessionService();
    public InvoiceService InvoiceService { get; } = new InvoiceService();
    public Stripe.RefundService RefundService { get; } = new Stripe.RefundService();
    public CustomerBalanceTransactionService CustomerBalanceTransactionService { get; } = new CustomerBalanceTransactionService();
    public PromotionCodeService PromotionCodeService { get; } = new PromotionCodeService();
    public DiscountService DiscountService { get; } = new DiscountService();
    public PaymentIntentService PaymentIntentService { get; } = new PaymentIntentService();
    public ChargeService ChargeService {  get; } = new ChargeService();

    private readonly IStripeCardFingerprintRepository _stripeCardFingerprintRepository;
    private readonly IStripeCustomerRepository _stripeCustomerRepository;
    private readonly string? _testClockId;
    private readonly string _publishableKey;

    public StripeService(IOptions<StripeOptions> stripeOptions, IStripeCardFingerprintRepository stripeCardFingerprintRepository, IStripeCustomerRepository stripeCustomerRepository)
    {
        StripeConfiguration.ApiKey = stripeOptions.Value.SecretKey;
        _testClockId = stripeOptions.Value.TestClockId;
        _stripeCardFingerprintRepository = stripeCardFingerprintRepository;
        _publishableKey = stripeOptions.Value.PublishableKey;
        _stripeCustomerRepository = stripeCustomerRepository;
    }

    public string GetPublishableKey()
    {
        return _publishableKey;
    }

    public async Task<StripeCustomer> CreateCustomerAsync(Guid userId, string userName, string email, string? phoneNumber, CancellationToken cancellationToken)
    {
        var customerCreateOptions = new CustomerCreateOptions()
        {
            Name = userName,
            Email = email,
            Phone = phoneNumber,
            Description = "Atlas Customer",
            Metadata = new Dictionary<string, string>()
            {
                { "Id", userId.ToString() }, // Set the user Id in the meta-data
            },
            TestClock = Utils.IsDevelopment() ? _testClockId : null, // If in development, set the test clock
        };

        Customer customer = await CustomerService.CreateAsync(customerCreateOptions, cancellationToken: cancellationToken);

        return StripeCustomer.Create(userId, customer.Id);
    }

    public async Task UpdateCustomerAsync(Guid userId, string userName, string email, string? phoneNumber, CancellationToken cancellationToken)
    {
        StripeCustomer stripeCustomer = await _stripeCustomerRepository.GetByUserId(userId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        var customerUpdateOptions = new CustomerUpdateOptions()
        {
            Name = userName,
            Phone = phoneNumber,
            Email = email,
        };
        await CustomerService.UpdateAsync(stripeCustomer.StripeCustomerId, customerUpdateOptions, null, cancellationToken);
    }

    public async Task DeleteCustomerAsync(string stripeCustomerId, CancellationToken cancellationToken)
    {
        Subscription? subscription = await GetStripeCustomerSubscriptionAsync(stripeCustomerId, cancellationToken);

        if (subscription is not null)
        {
            await CancelSubscriptionImmediatelyAsync(subscription.Id, cancellationToken);
        }

        await CustomerService.DeleteAsync(stripeCustomerId, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets the <see cref="Subscription"/> for a <see cref="User"/>. If the user does not have a subscription, null is returned.
    /// </summary>
    /// <param name="stripeCustomerId">The Id of the <see cref="Subscription"/></param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <param name="expand">An optional array of related entities that can be returned with the <see cref="Subscription"/></param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete, returning a <see cref="Subscription"/>.</returns>
    public async Task<Subscription?> GetStripeCustomerSubscriptionAsync(string stripeCustomerId, CancellationToken cancellationToken, IEnumerable<string>? expand = null)
    {
        var subscriptionListOptions = new SubscriptionListOptions
        {
            Customer = stripeCustomerId,
        };

        if (expand is not null)
            subscriptionListOptions.Expand = expand.ToList();

        // Get all subscriptions for the customer, excluding incomplete subscriptions
        IEnumerable<Subscription> subscriptions = (await SubscriptionService.ListAsync(subscriptionListOptions, cancellationToken: cancellationToken))
            .Where(x => !x.Status.Contains("incomplete"));

        return subscriptions.SingleOrDefault();
    }

    /// <summary>
    /// Gets the <see cref="PromotionCode"/> from a promotion code string.
    /// </summary>
    /// <param name="promotionCode">The promotion code with a code of <paramref name="promotionCode"/>.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete, returning a <see cref="Coupon"/> or <c>null</c> if there is no coupon with promotion code equal to <paramref name="promotionCode"/>.</returns>
    public async Task<PromotionCode?> GetPromotionCodeFromPromotionCodeAsync(string promotionCode, CancellationToken cancellationToken)
    {
        var promotionCodeListOptions = new PromotionCodeListOptions()
        {
            Limit = 1,
            Code = promotionCode
        };
        PromotionCode? promotionCodeObject = (await PromotionCodeService.ListAsync(promotionCodeListOptions, cancellationToken: cancellationToken)).FirstOrDefault();
        return promotionCodeObject;
    }

    /// <summary>
    /// Voids all open invoices for a <see cref="Customer"/>.
    /// </summary>
    /// <param name="stripeCustomerId">The Id of the <see cref="Customer"/>.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    public async Task VoidAllOpenInvoicesAsync(string stripeCustomerId, CancellationToken cancellationToken)
    {
        var invoiceListOptions = new InvoiceListOptions
        {
            Customer = stripeCustomerId,
            Status = "open"
        };
        IEnumerable<Invoice> openInvoices = await InvoiceService.ListAsync(invoiceListOptions, cancellationToken: cancellationToken);
        foreach (Invoice openInvoice in openInvoices)
            await InvoiceService.VoidInvoiceAsync(openInvoice.Id, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Retrieves the <see cref="Price"/>s of a <see cref="Product"/>.
    /// </summary>
    /// <param name="stripeProductId">The ID of the Stripe product.</param>
    /// <param name="includeInactive">Flag indicating whether to include inactive prices.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete, returning the collection of Stripe Prices.</returns>
    public async Task<IEnumerable<Price>> GetProductPrices(string stripeProductId, bool includeInactive, CancellationToken cancellationToken)
    {
        var priceListOptions = new PriceListOptions
        {
            Product = stripeProductId,
            Active = includeInactive ? null : true,
        };
        return await StripePriceService.ListAsync(priceListOptions, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Retrieves whether a user is eligible for a trial of a given plan.
    /// </summary>
    /// <param name="stripeCustomerId">The ID of the Strupe Customer for which to check the eligibility.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete, returning a boolean that indicated whether the user is eligible for a trial.</returns>
    public async Task<bool> IsUserEligibleForTrialAsync(string stripeCustomerId, CancellationToken cancellationToken)
    {
        var subscriptionListOptions = new SubscriptionListOptions
        {
            Customer = stripeCustomerId,
            Status = "all"
        };

        // Get all subscriptions for the customer
        IEnumerable<Subscription> subscriptions = await SubscriptionService.ListAsync(subscriptionListOptions, cancellationToken: cancellationToken);

        // If the user has any past subscriptions, that aren't incomplete (Maybe an abandoned subscription at the 3D Secure stage?). 
        bool anyPreviousSubscriptions = subscriptions.Any(x => x.Status != SubscriptionStatuses.Incomplete);

        return !anyPreviousSubscriptions;
    }

    public async Task<bool> HasCardPaymentMethodBeenUsedBeforeAsync(string paymentMethodId, CancellationToken cancellationToken)
    {
        PaymentMethod paymentMethod = await PaymentMethodService.GetAsync(paymentMethodId, cancellationToken: cancellationToken);
        StripeCardFingerprint? cardFingerprint = await _stripeCardFingerprintRepository.GetByFingerprintAsync(paymentMethod.Card.Fingerprint, false, cancellationToken);
        return cardFingerprint is not null;
    }

    /// <summary>
    /// If the user is downgrading, this method will refund them the difference between the old and new plan. 
    /// </summary>
    /// <param name="subscription">The updated <see cref="Subscription"/>.</param>
    /// <param name="stripeCustomerId">The Id of the stripe customer <paramref name="subscription"/> belongs to.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete./returns>
    public async Task ProcessRefundIfNecessaryAsync(Subscription subscription, string stripeCustomerId, CancellationToken cancellationToken)
    {
        // Calculate the amount to refund
        Invoice latestInvoice = await InvoiceService.GetAsync(subscription.LatestInvoiceId, cancellationToken: cancellationToken);
        long totalAmountToRefund = latestInvoice.Total * -1;

        if (totalAmountToRefund <= 0)
            return;

        // Get the invoices where a charge was made 
        var invoiceListOptions = new InvoiceListOptions()
        {
            Customer = stripeCustomerId,
            Status = "paid",
            Expand = new List<string> { "data.charge" }
        };
        IEnumerable<Invoice> invoices = await InvoiceService.ListAsync(invoiceListOptions, cancellationToken: cancellationToken);
        invoices = invoices.Where(x => x.Charge is not null && x.Charge.Amount > 0).OrderByDescending(x => x.Created).ToList();

        // Chip away at the amount to refund by looping thorugh the invoices
        foreach (Invoice invoice in invoices)
        {
            if (totalAmountToRefund <= 0)
                break;

            // The amount to refund per invoice. Clamp to the totalAmountToRefund, to ensure we don't over refund
            long invoiceAmountToRefund = Math.Clamp(invoice.Charge.Amount - invoice.Charge.AmountRefunded, 0, totalAmountToRefund);
            if (invoiceAmountToRefund <= 0)
                continue;

            var refundCreateOptions = new RefundCreateOptions()
            {
                Amount = invoiceAmountToRefund,
                Charge = invoice.ChargeId
            };
            Refund refund = await RefundService.CreateAsync(refundCreateOptions, cancellationToken: cancellationToken);

            totalAmountToRefund -= invoiceAmountToRefund;

            // Assuming the refund hasn't thrown an exception, minus the amount to refund from the Stripe Customer's credit balance
            var customerBalanceTransactionCreateOptions = new CustomerBalanceTransactionCreateOptions
            {
                Amount = invoiceAmountToRefund,
                Currency = subscription.Currency
            };
            await CustomerBalanceTransactionService.CreateAsync(stripeCustomerId, customerBalanceTransactionCreateOptions, cancellationToken: cancellationToken);
        }
    }

    public async Task SetSubscriptionCancelAtPeriodEnd(string stripeCustomerId, bool cancelAtPeriodEnd, CancellationToken cancellationToken)
    {
        // Get Subscription
        var subscriptionListOptions = new SubscriptionListOptions
        {
            Customer = stripeCustomerId
        };
        IEnumerable<Subscription> subscriptions = await SubscriptionService.ListAsync(subscriptionListOptions, cancellationToken: cancellationToken);
        Subscription subscription = subscriptions.FirstOrDefault()
            ?? throw new ErrorException(PlansDomainErrors.Stripe.UserDoesNotHaveSubscription);

        // Update subscription to cancel at period end
        var subscriptionUpdateOptions = new SubscriptionUpdateOptions
        {
            CancelAtPeriodEnd = cancelAtPeriodEnd
        };

        await SubscriptionService.UpdateAsync(subscription.Id, subscriptionUpdateOptions, cancellationToken: cancellationToken);
    }

    public async Task CancelUserSubscriptionImmediatelyAsync(string stripeCustomerId, CancellationToken cancellationToken)
    {
        // Get user
        Subscription subscription = await GetStripeCustomerSubscriptionAsync(stripeCustomerId, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Stripe.UserDoesNotHaveSubscription);

        await CancelSubscriptionImmediatelyAsync(subscription.Id, cancellationToken);
    }

    public async Task CancelSubscriptionImmediatelyAsync(string subscriptionId, CancellationToken cancellationToken)
    {
        var subscriptionCancelOptions = new SubscriptionCancelOptions
        {
            Prorate = false,
        };

        try
        {
            await SubscriptionService.CancelAsync(subscriptionId, subscriptionCancelOptions, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ErrorException(PlansDomainErrors.Stripe.UnknownError(ex.Message));
        }
    }

    public async Task<string> CreateBillingPortalLinkAsync(string stripeCustomerId, CancellationToken cancellationToken)
    {
        var sessionCreateOptions = new Stripe.BillingPortal.SessionCreateOptions()
        {
            Customer = stripeCustomerId,
            ReturnUrl = $"{Utils.GetWebsiteUrl()}/account-settings/manage-subscription",
        };

        Stripe.BillingPortal.Session session = await BillingSessionService.CreateAsync(sessionCreateOptions, cancellationToken: cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Stripe.CouldNotCreateCustomerBillingPortalLink);

        return session.Url;
    }

    public async Task SetSubscriptionPaymentMethodAsync(string subscriptionId, string paymentMethodId, CancellationToken cancellationToken)
    {
        // Just a sanity check to ensure the payment method actually exists. We actually use the returned PaymentMethod object.
        PaymentMethod _ = await PaymentMethodService.GetAsync(paymentMethodId, cancellationToken: cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Stripe.PaymentMethodNotFound);

        Subscription subscription = await SubscriptionService.GetAsync(subscriptionId, cancellationToken: cancellationToken);

        var subscriptionUpdateOptions = new SubscriptionUpdateOptions
        {
            DefaultPaymentMethod = paymentMethodId
        };
        await SubscriptionService.UpdateAsync(subscription.Id, subscriptionUpdateOptions, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets the <see cref="Coupon"/> from a promotion code
    /// </summary>
    /// <param name="promotionCode">The promotion code from the <see cref="Coupon"/>.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete, returning a <see cref="Coupon"/> or <c>null</c> if there is no coupon with promotion code equal to <paramref name="promotionCode"/>.</returns>
    public async Task<Coupon?> GetCouponFromPromotionCodeAsync(string promotionCode, CancellationToken cancellationToken)
    {
        var promotionCodeListOptions = new PromotionCodeListOptions()
        {
            Limit = 1,
            Code = promotionCode,
            Expand = new List<string> { "data.coupon" }
        };
        PromotionCode? promotionCodeObject = (await PromotionCodeService.ListAsync(promotionCodeListOptions, cancellationToken: cancellationToken)).FirstOrDefault();

        return promotionCodeObject?.Coupon;
    }

    /// <summary>
    /// Creates a <see cref="Product"/>.
    /// </summary>
    /// <param name="plan">The plan details associated with the product.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete, returning the created Stripe Product ID.</returns>
    public async Task<string> CreateStripeProductAsync(AtlasPlan plan, CancellationToken cancellationToken)
    {
        var productOptions = new ProductCreateOptions
        {
            Name = plan.Name,
            Description = plan.Description,
            Active = true,
        };

        Product stripeProduct = await StripeProductService.CreateAsync(productOptions, cancellationToken: cancellationToken);
        return stripeProduct.Id;
    }

    /// <summary>
    /// Creates a <see cref="Stripe.Price"/> for a given plan and interval type.
    /// </summary>
    /// <param name="plan">The plan details associated with the price.</param>
    /// <param name="intervalType">The interval type for the price (e.g., monthly, yearly).</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete, returning the created Stripe Price ID.</returns>
    public async Task<string> CreateStripePriceAsync(AtlasPlan plan, string intervalType, CancellationToken cancellationToken)
    {
        if (intervalType != "month" && intervalType != "year")
        {
            throw new ErrorException(PlansDomainErrors.Stripe.InvalidSubscriptionInterval);
        }

        var priceOptions = new PriceCreateOptions
        {
            Product = plan.StripeProductId,
            Active = plan.Active,
            Currency = plan.IsoCurrencyCode,
            Recurring = new PriceRecurringOptions
            {
                Interval = intervalType,
                IntervalCount = 1,                      // Every 1 month or 1 year
                UsageType = "licensed",
            },
            UnitAmount = intervalType == "month" ? plan.MonthlyPrice : plan.AnnualPrice,
            TaxBehavior = "inclusive",                  // Include the tax in the price of the product
        };

        Price price = await StripePriceService.CreateAsync(priceOptions, cancellationToken: cancellationToken);
        return price.Id;
    }

    /// <summary>
    /// Updates a <see cref="Product"/>.
    /// </summary>
    /// <param name="plan">The plan details associated with the product update.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    public async Task UpdateProductAsync(AtlasPlan plan, CancellationToken cancellationToken)
    {
        var productUpdateOptions = new ProductUpdateOptions
        {
            Name = plan.Name,
            Description = plan.Description,
            Active = plan.Active
        };

        var productService = new ProductService();
        await productService.UpdateAsync(plan.StripeProductId, productUpdateOptions, null, cancellationToken);
    }

    public async Task DeactivateAllPricesAsync(AtlasPlan plan, CancellationToken cancellationToken)
    {
        IEnumerable<Price> activePrices = await GetProductPrices(plan.StripeProductId, false, cancellationToken);
        foreach (var price in activePrices)
        {
            await StripePriceService.UpdateAsync(price.Id, new PriceUpdateOptions()
            {
                Active = false
            }, null, cancellationToken);
        }
    }

    public async Task CreatePricesForPlanAsync(AtlasPlan plan, CancellationToken cancellationToken)
    {
        await CreateStripePriceAsync(plan, "month", cancellationToken);
        await CreateStripePriceAsync(plan, "year", cancellationToken);
    }

    /// <summary>
    /// Handles the changing of a plan price at a particular interval. 
    /// </summary>
    /// <param name="plan">The <see cref="Plan"/> to change the price of.</param>
    /// <param name="intervalType">Must be either month or year</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    public async Task UpgradePriceAtIntervalAsync(AtlasPlan plan, string intervalType, CancellationToken cancellationToken)
    {
        if (intervalType != "month" && intervalType != "year")
        {
            throw new ErrorException(PlansDomainErrors.Stripe.InvalidSubscriptionInterval);
        }

        var priceListOptions = new PriceListOptions
        {
            Product = plan.StripeProductId,
            Active = true,
            Recurring = new PriceRecurringListOptions()
            {
                Interval = intervalType,
            }
        };
        IEnumerable<Price> prices = await StripePriceService.ListAsync(priceListOptions, cancellationToken: cancellationToken);
        Price price = prices.First(); // There should only ever be ONE price at any interval on a plan

        // Deactivate this price first
        await StripePriceService.UpdateAsync(price.Id, new PriceUpdateOptions()
        {
            Active = false
        }, null, cancellationToken);


        // Create a NEW price
        string newPriceId = await CreateStripePriceAsync(plan, intervalType, cancellationToken);

        await UpdateSubscriptionsToNewPriceAsync(price.Id, newPriceId, cancellationToken);
    }

    /// <summary>
    /// Determines if there are any <see cref="Subscription"/>s linked to the <paramref name="plan"/>.
    /// </summary>
    /// <param name="plan">The <see cref="Plan"/> to check for subscribers for.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>True if there are users subscribed to the <paramref name="plan"/>, false otherwise.</returns>
    public async Task<bool> DoesPlanHaveActiveSubscriptions(AtlasPlan plan, CancellationToken cancellationToken)
    {
        var subscriptionSearchOptions = new SubscriptionSearchOptions
        {
            Limit = 1,
            Query = $"status:\"active\" AND metadata[\"planId\"]:\"{plan.Id}\""
        };

        var results = await SubscriptionService.SearchAsync(subscriptionSearchOptions, null, cancellationToken);

        return results.Any();
    }

    /// <summary>
    /// When a price is updated, we need to update all subscriptions that are using that price to the new price. This method achieves that.
    /// </summary>
    /// <param name="oldPriceId">The old <see cref="Price"/> Id.</param>
    /// <param name="newPriceId">The new <see cref="Price"/> Id, to which pre-existing <see cref="Subscription"/>s should be updated to.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    private async Task UpdateSubscriptionsToNewPriceAsync(string oldPriceId, string newPriceId, CancellationToken cancellationToken)
    {
        string? startAfterCursor = null;

        // Loop through all Subscriptions. We can only fetch 100 at once, so we must have this loop with a startAfter cursor
        while (true)
        {
            var options = new SubscriptionListOptions
            {
                Limit = 100,
                Price = oldPriceId,
                TestClock = Utils.IsDevelopment() ? _testClockId : null, // If in development, set the test clock
                StartingAfter = startAfterCursor,
            };

            IEnumerable<Subscription> subscriptions = (await SubscriptionService.ListAsync(options, cancellationToken: cancellationToken)).ToList();
            if (subscriptions is null || !subscriptions.Any())
                break;

            // Set the cursor to the last Subscription
            startAfterCursor = subscriptions.Last().Id;

            foreach (Subscription subscription in subscriptions)
            {
                // Update the subscription, passing in the new price Id
                var subscriptionUpdateOptions = new SubscriptionUpdateOptions
                {
                    Items = new List<SubscriptionItemOptions>
                    {
                        // Update the existing Subscription item with the new price.
                        new SubscriptionItemOptions
                        {
                            Id = subscription.Items.First().Id,
                            Price = newPriceId
                        }
                    },
                    ProrationBehavior = "none",
                };

                await SubscriptionService.UpdateAsync(subscription.Id, subscriptionUpdateOptions, cancellationToken: cancellationToken);
            }
        }
    }
}
