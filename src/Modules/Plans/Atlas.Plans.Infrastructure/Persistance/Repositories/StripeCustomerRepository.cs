using Atlas.Infrastructure.Persistance;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Plans.Infrastructure.Persistance.Repositories;

internal sealed class StripeCustomerRepository(PlansDatabaseContext context)
    : Repository<StripeCustomer, PlansDatabaseContext>(context), IStripeCustomerRepository
{
    public async Task<StripeCustomer?> GetByUserId(Guid UserId, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IQueryable<StripeCustomer> query = GetDbSet(trackChanges);

        return await query.FirstOrDefaultAsync(x => x.UserId == UserId, cancellationToken);
    }

    public async Task<StripeCustomer?> GetByStripeCustomerId(string stripeCustomerId, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IQueryable<StripeCustomer> query = GetDbSet(trackChanges);

        return await query.FirstOrDefaultAsync(x => x.StripeCustomerId == stripeCustomerId, cancellationToken);
    }
}