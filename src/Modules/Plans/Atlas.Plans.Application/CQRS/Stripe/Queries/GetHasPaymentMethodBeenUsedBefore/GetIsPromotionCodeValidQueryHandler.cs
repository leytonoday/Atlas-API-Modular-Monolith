﻿using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;
using Atlas.Plans.Domain.Services;
using MediatR;
using Stripe;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetHasPaymentMethodBeenUsedBefore;

internal sealed class GetHasPaymentMethodBeenUsedBeforeQueryHandler(IStripeService stripeService, IPlansUnitOfWork unitOfWork) : IRequestHandler<GetHasPaymentMethodBeenUsedBeforeQuery, bool>
{
    public async Task<bool> Handle(GetHasPaymentMethodBeenUsedBeforeQuery request, CancellationToken cancellationToken)
    {
        PaymentMethod paymentMethod = await stripeService.PaymentMethodService.GetAsync(request.PaymentMethodId, cancellationToken: cancellationToken);

        // If we haven't stored this fingerprint before, then we know this card hasn't been used previously in Atlas
        StripeCardFingerprint? cardFingerprint = await unitOfWork.StripeCardFingerprintRepository.GetByFingerprintAsync(paymentMethod.Card.Fingerprint, false, cancellationToken);

        return cardFingerprint is not null;
    }
}