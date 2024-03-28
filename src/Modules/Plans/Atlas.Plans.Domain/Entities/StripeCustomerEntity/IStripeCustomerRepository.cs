using Atlas.Shared.Domain.Persistance;

namespace Atlas.Plans.Domain.Entities.StripeCustomerEntity;

public interface IStripeCustomerRepository : IRepository<StripeCustomer>
{
    public Task<StripeCustomer?> GetByUserId(Guid UserId, bool trackChanges, CancellationToken cancellationToken);

    public Task<StripeCustomer?> GetByStripeCustomerId(string stripeCustomerId, bool trackChanges, CancellationToken cancellationToken);
}