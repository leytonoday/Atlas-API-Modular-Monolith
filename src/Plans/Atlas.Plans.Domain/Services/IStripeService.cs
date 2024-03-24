using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Users.Domain.Entities.UserEntity;
using Stripe;

namespace Atlas.Plans.Domain.Services;

using AtlasPlan = Atlas.Plans.Domain.Entities.PlanEntity.Plan;

public interface IStripeService
{
    // Exposing raw stripe classes for convenience
    public ProductService StripeProductService { get; }
    public PriceService StripePriceService { get; }
    public Stripe.CustomerService CustomerService { get; }
    public SubscriptionService SubscriptionService { get; }
    public PaymentMethodService PaymentMethodService { get; }
    public Stripe.BillingPortal.SessionService BillingSessionService { get; }
    public InvoiceService InvoiceService { get; }
    public Stripe.RefundService RefundService { get; }
    public CustomerBalanceTransactionService CustomerBalanceTransactionService { get; }
    public PromotionCodeService PromotionCodeService { get; }
    public DiscountService DiscountService { get; }

    public PaymentIntentService PaymentIntentService { get; }
    public ChargeService ChargeService { get; }

    public string GetPublishableKey();

    public Task<StripeCustomer> CreateCustomerAsync(User user, CancellationToken cancellationToken);

    public Task UpdateCustomerAsync(User user, CancellationToken cancellationToken);

    public Task DeleteCustomerAsync(string stripeCustomerId, CancellationToken cancellationToken);

    public Task<Subscription?> GetStripeCustomerSubscriptionAsync(string stripeCustomerId, CancellationToken cancellationToken, IEnumerable<string>? expand = null);

    public Task<PromotionCode?> GetPromotionCodeFromPromotionCodeAsync(string promotionCode, CancellationToken cancellationToken);

    public Task VoidAllOpenInvoicesAsync(string stripeCustomerId, CancellationToken cancellationToken);

    public Task<IEnumerable<Price>> GetProductPrices(string stripeProductId, bool includeInactive, CancellationToken cancellationToken);

    public Task<bool> IsUserEligibleForTrialAsync(string stripeCustomerId, CancellationToken cancellationToken);

    public Task<bool> HasCardPaymentMethodBeenUsedBeforeAsync(string paymentMethodId, CancellationToken cancellationToken);

    public Task ProcessRefundIfNecessaryAsync(Subscription subscription, string stripeCustomerId, CancellationToken cancellationToken);

    public Task SetSubscriptionCancelAtPeriodEnd(string stripeCustomerId, bool cancelAtPeriodEnd, CancellationToken cancellationToken);

    public Task CancelUserSubscriptionImmediatelyAsync(string stripeCustomerId, CancellationToken cancellationToken);

    public Task CancelSubscriptionImmediatelyAsync(string subscriptionId, CancellationToken cancellationToken);

    public Task<string> CreateBillingPortalLinkAsync(string stripeCustomerId, CancellationToken cancellationToken);

    public Task SetSubscriptionPaymentMethodAsync(string subscriptionId, string paymentMethodId, CancellationToken cancellationToken);

    public Task<Coupon?> GetCouponFromPromotionCodeAsync(string promotionCode, CancellationToken cancellationToken);

    public Task<string> CreateStripeProductAsync(AtlasPlan plan, CancellationToken cancellationToken);

    public Task<string> CreateStripePriceAsync(AtlasPlan plan, string intervalType, CancellationToken cancellationToken);

    public Task UpdateProductAsync(AtlasPlan plan, CancellationToken cancellationToken);

    public Task DeactivateAllPricesAsync(AtlasPlan plan, CancellationToken cancellationToken);

    public Task CreatePricesForPlanAsync(AtlasPlan plan, CancellationToken cancellationToken);

    public Task UpgradePriceAtIntervalAsync(AtlasPlan plan, string intervalType, CancellationToken cancellationToken);

    public Task<bool> DoesPlanHaveActiveSubscriptions(AtlasPlan plan, CancellationToken cancellationToken);
}
