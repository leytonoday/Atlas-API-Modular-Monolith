using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;
using Atlas.Plans.Domain.Entities.StripeCustomerEntity;
using Atlas.Shared.Domain;

namespace Atlas.Plans.Domain;

public interface IPlansUnitOfWork : IBaseUnitOfWork
{
    public IPlanRepository PlanRepository { get; }

    public IFeatureRepository FeatureRepository { get; }

    public IPlanFeatureRepository PlanFeatureRepository { get; }

    public IStripeCustomerRepository StripeCustomerRepository { get; }

    public IStripeCardFingerprintRepository StripeCardFingerprintRepository { get; }
}
