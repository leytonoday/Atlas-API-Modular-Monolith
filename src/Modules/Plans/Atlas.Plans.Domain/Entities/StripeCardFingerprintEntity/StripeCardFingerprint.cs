using Atlas.Shared.Domain.Entities;

namespace Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;

/// <summary>
/// Represents a Stripe Card Fingerprint entity, which corresponds to a single <see cref="Stripe.PaymentMethodCard"/>.
/// </summary>
/// <remarks>Used to track which cards have been used to make payments on Atlas, to prevent users from exploiting the free 
/// trial. When the user provides a card, if it's fingerprint is in the database, then the user cannot claim a free trial with that card.</remarks>
public class StripeCardFingerprint : Entity
{
    private StripeCardFingerprint() { }

    /// <summary>
    /// Gets or sets a fingerprint, which uniquely identifies a single card payment method.
    /// </summary>
    public string Fingerprint { get; private set; } = null!;

    public static StripeCardFingerprint Create(string fingerprint)
    {
        return new StripeCardFingerprint() { Fingerprint = fingerprint };
    }
}
