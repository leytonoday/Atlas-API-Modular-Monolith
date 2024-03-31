using Atlas.Shared.Application.Abstractions.Messaging.Command;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.AttachPaymentMethod;

public sealed record AttachPaymentMethodCommand(Guid UserId, string StripePaymentMethodId, bool SetAsDefaultPaymentMethod) : ICommand;