using Atlas.Plans.Application.CQRS.Stripe.Shared;
using AutoMapper;

namespace Atlas.Plans.Infrastructure.MappingProfiles;

/// <summary>
/// Represents a set of AutoMapper mapping profiles for <see cref="Stripe"/> entities and their DTOs.
/// </summary>
public class StripeMappingProfile : Profile
{
    /// <summary>
    /// Initialises a new instance of the <see cref="StripeMappingProfile"/> class.
    /// </summary>
    public StripeMappingProfile()
    {
        CreateMap<Stripe.Invoice, StripeSlimInvoiceDto>();
    }
}
