using Atlas.Shared.Application.Abstractions.Messaging.Query;
using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetCustomerPortalLink;

public sealed record GetCustomerPortalLinkQuery(Guid UserId) : IQuery<string>;