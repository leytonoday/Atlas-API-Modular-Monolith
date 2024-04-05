using MediatR;
using Stripe;
using Atlas.Plans.Application.CQRS.Stripe.Shared;
using AutoMapper;
using Atlas.Plans.Domain.Services;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Application.Abstractions;

namespace Atlas.Plans.Application.CQRS.Stripe.Queries.GetInvoiceHistory;

internal sealed class GetInvoiceHistoryQueryHandler(IStripeCustomerRepository stripeCustomerRepository, IStripeService stripeService, IExecutionContextAccessor executionContext, IMapper mapper) : IQueryHandler<GetInvoiceHistoryQuery, IEnumerable<StripeSlimInvoiceDto>>
{
    public async Task<IEnumerable<StripeSlimInvoiceDto>> Handle(GetInvoiceHistoryQuery request, CancellationToken cancellationToken)
    {
        StripeCustomer? stripeCustomer = await stripeCustomerRepository.GetByUserId(executionContext.UserId, false, cancellationToken)
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
