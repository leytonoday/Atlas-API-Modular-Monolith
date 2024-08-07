﻿using MediatR;
using Stripe;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Abstractions;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.AttachPaymentMethod;

internal sealed class AttachPaymentMethodCommandHandler(IStripeCustomerRepository stripeCustomerRepository, IExecutionContextAccessor executionContext, IStripeService stripeService) : ICommandHandler<AttachPaymentMethodCommand>
{
    public async Task Handle(AttachPaymentMethodCommand request, CancellationToken cancellationToken)
    {
        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(executionContext.UserId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        var paymentMethodAttachOptions = new PaymentMethodAttachOptions
        {
            Customer = stripeCustomer.StripeCustomerId
        };

        try
        {
            await stripeService.PaymentMethodService.AttachAsync(request.StripePaymentMethodId, paymentMethodAttachOptions, cancellationToken: cancellationToken);
        }
        catch (StripeException e)
        {
            throw new ErrorException(PlansDomainErrors.Stripe.UnknownError(e.Message));
        }

        if (request.SetAsDefaultPaymentMethod)
        {
            var customerUpdateOptions = new CustomerUpdateOptions
            {
                InvoiceSettings = new CustomerInvoiceSettingsOptions()
                {
                    DefaultPaymentMethod = request.StripePaymentMethodId
                }
            };
            await stripeService.CustomerService.UpdateAsync(stripeCustomer.StripeCustomerId, customerUpdateOptions, cancellationToken: cancellationToken);
        }

    }
}
