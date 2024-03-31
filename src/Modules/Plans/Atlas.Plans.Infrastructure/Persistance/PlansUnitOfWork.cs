using Atlas.Plans.Domain;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Plans.Infrastructure.Persistance.Repositories;
using Atlas.Shared.Infrastructure.Persistance;
using Microsoft.Extensions.Logging;

namespace Atlas.Plans.Infrastructure.Persistance;

internal sealed class PlansUnitOfWork(PlansDatabaseContext plansDatabaseContext, ILogger<PlansUnitOfWork> logger) 
    : BaseUnitOfWork<PlansDatabaseContext, ILogger<PlansUnitOfWork>>(plansDatabaseContext, logger), 
    IPlansUnitOfWork
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
}
