using MediatR;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Atlas.Plans.Domain;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Errors;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Users.Application.Abstractions;

namespace Atlas.Plans.Application.CQRS.Stripe.Commands.AttachPaymentMethod;

internal sealed class AttachPaymentMethodCommandHandler(IPlansUnitOfWork unitOfWork, UserManager<User> userManager, IUserContext userContext, IStripeService stripeService) : IRequestHandler<AttachPaymentMethodCommand>
{
    public async Task Handle(AttachPaymentMethodCommand request, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByIdAsync(userContext.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        StripeCustomer? stripeCustomer = await unitOfWork.StripeCustomerRepository.GetByUserId(user.Id, false, cancellationToken)
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
