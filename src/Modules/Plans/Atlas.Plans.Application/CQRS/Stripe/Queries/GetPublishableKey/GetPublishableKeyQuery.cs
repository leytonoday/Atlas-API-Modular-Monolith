using Atlas.Shared.Application.Abstractions.Messaging.Query;
using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetPublishableKey;

public sealed record GetPublishableKeyQuery() : IQuery<string>;