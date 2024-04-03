using Atlas.Shared.Domain.Errors;

namespace Atlas.Plans.Domain.Errors;

public static class PlansDomainErrors
{
    /// <summary>
    /// Creates a standard error code for the <see cref="Error"/> class by combining the name of the class and the name of the property separated by a dot. e.g., User.EmailAlreadyInUse
    /// </summary>
    /// <param name="type">The type on which the error has occured.</param>
    /// <param name="propertyName">The name of the property that calls the method.</param>
    /// <returns>An error code for the <see cref="Error"/> class.</returns>
    private static string CreateErrorCode(Type type, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
    {
        return $"{type.Name}.{propertyName}";
    }

    public static class Feature
    {
        public static readonly Error FeatureNotFound = new(CreateErrorCode(typeof(Feature)), "The Feature with the specified Id does not exist.");

    }

    public static class Plan
    {
        public static readonly Error PlanNotFound = new(CreateErrorCode(typeof(Plan)), "The Plan with the specified Id does not exist.");
    }

    public static class PlanFeature
    {
        public static readonly Error PlanFeatureNotFound = new(CreateErrorCode(typeof(PlanFeature)), "The PlanFeature with the specified planId and featureId does not exist.");
    }

    public static class Stripe
    {
        public static readonly Error InvalidSubscriptionInterval = new(CreateErrorCode(typeof(Stripe)), "The subscription interval must be 'month' or 'year'.");

        public static readonly Error UserAlreadySubscribedToPlan = new(CreateErrorCode(typeof(Stripe)), "The given User is already subscribed to the given Plan.");

        public static readonly Error InvalidPromotionCode = new(CreateErrorCode(typeof(Stripe)), "The given promotion code is invalid.");

        public static readonly Error UserDoesNotHaveSubscription = new(CreateErrorCode(typeof(Stripe)), "The given user does not have a subscription.");

        public static readonly Error UserHasPastDueSubscription = new(CreateErrorCode(typeof(Stripe)), "The given user has a past due subscription.");

        public static readonly Error PaymentMethodNotFound = new(CreateErrorCode(typeof(Stripe)), "The payment method with the given Id could not be found.");

        public static readonly Error NoDefaultPaymentMethod = new(CreateErrorCode(typeof(Stripe)), "The user does not have the a default payment method.");

        public static Error UnknownError(string message) => new(CreateErrorCode(typeof(Stripe)), message);

        public static readonly Error CouldNotCreateCustomerBillingPortalLink = new(CreateErrorCode(typeof(Stripe)), "Could not create Customer portal billing link.");
    }

    public static class StripeCustomer
    {
        public static readonly Error StripeCustomerNotFound = new(CreateErrorCode(typeof(StripeCustomer)), "The User does not have a corresponding Stripe Customer.");
    }
}