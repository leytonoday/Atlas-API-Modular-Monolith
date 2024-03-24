using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetPublishableKey;

public sealed record GetPublishableKeyQuery() : IRequest<string>;