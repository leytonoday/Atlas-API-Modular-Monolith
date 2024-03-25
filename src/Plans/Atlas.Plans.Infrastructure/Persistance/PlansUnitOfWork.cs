using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Infrastructure.Persistance.Entities;
using Atlas.Plans.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Atlas.Plans.Infrastructure.Persistance;

internal sealed class PlansUnitOfWork(PlansDatabaseContext plansDatabaseContext) : IPlansUnitOfWork
{
    public IPlanRepository PlanRepository => _planRepository.Value;
    private readonly Lazy<IPlanRepository> _planRepository = new(() => new PlanRepository(plansDatabaseContext));

    public IFeatureRepository FeatureRepository => _featureRepository.Value;
    private readonly Lazy<IFeatureRepository> _featureRepository = new(() => new FeatureRepository(plansDatabaseContext));

    public IPlanFeatureRepository PlanFeatureRepository => _planFeatureRepository.Value;
    private readonly Lazy<IPlanFeatureRepository> _planFeatureRepository = new(() => new PlanFeatureRepository(plansDatabaseContext));

    public IStripeCustomerRepository StripeCustomerRepository => _stripeCustomerRepository.Value;
    private readonly Lazy<IStripeCustomerRepository> _stripeCustomerRepository = new(() => new StripeCustomerRepository(plansDatabaseContext));

    public IStripeCardFingerprintRepository StripeCardFingerprintRepository => _stripeCardFingerprintRepository.Value;
    private readonly Lazy<IStripeCardFingerprintRepository> _stripeCardFingerprintRepository = new(() => new StripeCardFingerprintRepository(plansDatabaseContext));

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        using var dbContextTransaction = plansDatabaseContext.Database.BeginTransaction();

        try
        {
            _ = await plansDatabaseContext.SaveChangesAsync(cancellationToken);
            await dbContextTransaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            //Log Exception Handling message                      
            await dbContextTransaction.RollbackAsync(cancellationToken);
        }
    }

    public async Task<bool> HasDomainEventBeenHandledAsync(string eventHandlerName, Guid domainEventId, CancellationToken cancellationToken)
    {
        return await plansDatabaseContext
            .Set<PlansOutboxMessageConsumerAcknowledgement>()
            .AsNoTracking()
            .AnyAsync(x => x.DomainEventId == domainEventId && x.EventHandlerName == eventHandlerName, cancellationToken);
    }

    public void MarkDomainEventAsHandled(string eventHandlerName, Guid domainEventId, CancellationToken cancellationToken)
    {
        plansDatabaseContext.Set<PlansOutboxMessageConsumerAcknowledgement>()
            .Add(new PlansOutboxMessageConsumerAcknowledgement()
            {
                DomainEventId = domainEventId,
                EventHandlerName = eventHandlerName
            });
    }
}
