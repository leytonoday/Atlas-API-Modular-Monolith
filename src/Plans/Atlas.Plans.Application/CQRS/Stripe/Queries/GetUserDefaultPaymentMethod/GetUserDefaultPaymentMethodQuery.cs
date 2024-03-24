﻿using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetUserDefaultPaymentMethod;

public sealed record GetUserDefaultPaymentMethodQuery(Guid UserId) : IRequest<PaymentMethod?>;