using Atlas.Shared.Domain.AggregateRoot;
using Atlas.Shared.Domain.Entities;

namespace Atlas.Plans.Domain.Entities.StripeCustomerEntity;

/// <summary>
/// Represents the link the <see cref="User"/> entity and it's corresponding Stripe Customer.
/// </summary>
public sealed class StripeCustomer : Entity, IAggregateRoot
{
    private StripeCustomer() { }

    public Guid UserId { get; private set; }

    public string StripeCustomerId { get; private set; } = null!;

    public static StripeCustomer Create(Guid userId, string stripeCustomerId)
    {
        return new StripeCustomer { UserId = userId, StripeCustomerId = stripeCustomerId };
    }
}
