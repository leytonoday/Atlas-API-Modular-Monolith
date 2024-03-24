using MediatR;
using Stripe;
using Atlas.Plans.Application.CQRS.Stripe.Shared;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Services;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Errors;
using Atlas.Plans.Domain.Errors;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetInvoiceHistory;

internal sealed class GetInvoiceHistoryQueryHandler(IPlansUnitOfWork unitOfWork, IStripeService stripeService, UserManager<User> userManager, IMapper mapper) : IRequestHandler<GetInvoiceHistoryQuery, IEnumerable<StripeSlimInvoiceDto>>
{
    public async Task<IEnumerable<StripeSlimInvoiceDto>> Handle(GetInvoiceHistoryQuery request, CancellationToken cancellationToken)
    {
        // Get user
        User user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        StripeCustomer? stripeCustomer = await unitOfWork.StripeCustomerRepository.GetByUserId(user.Id, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.StripeCustomer.StripeCustomerNotFound);

        var invoiceListOptions = new InvoiceListOptions
        {
            Customer = stripeCustomer.StripeCustomerId,
            Limit = request.Limit ?? 10,
            StartingAfter = request.StartingAfter,
        };

        IEnumerable<Invoice> invoices = (await stripeService.InvoiceService.ListAsync(invoiceListOptions, cancellationToken: cancellationToken)).Where(x => x.Status != "draft");
        IEnumerable<StripeSlimInvoiceDto> slimInvoices = mapper.Map<IEnumerable<StripeSlimInvoiceDto>>(invoices);

        return slimInvoices;
    }
}
