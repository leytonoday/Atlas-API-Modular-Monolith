using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Shared;

public class StripeSlimInvoiceDto
{
    /// <inheritdoc cref="Stripe.Invoice.Id"/>
    public string Id { get; set; }

    /// <inheritdoc cref="Stripe.Invoice.Currency"/>
    public string Currency { get; set; }

    /// <inheritdoc cref="Stripe.Invoice.Created"/>
    public DateTime Created { get; set; }

    /// <inheritdoc cref="Stripe.Invoice.HostedInvoiceUrl"/>
    public string HostedInvoiceUrl { get; set; }

    /// <inheritdoc cref="Stripe.Invoice.Status"/>
    public string Status { get; set; }

    /// <inheritdoc cref="Stripe.Invoice.Subtotal"/>
    public long Subtotal { get; set; }

    /// <inheritdoc cref="Stripe.Invoice.Total"/>
    public long Total { get; set; }

    /// <inheritdoc cref="Stripe.Invoice.Lines"/>
    public StripeList<InvoiceLineItem> Lines { get; set; }
}
