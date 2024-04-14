using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Shared.Domain.AggregateRoot;
using Atlas.Shared.Domain.Entities;
using Atlas.Shared.Domain.Exceptions;
using Newtonsoft.Json.Linq;

namespace Atlas.Plans.Domain.Entities.CreditTrackerEntity;

public sealed class CreditTracker: Entity, IAggregateRoot
{
    private CreditTracker() { }

    public Guid UserId { get; private set; }

    public int MaxCreditCount { get; private set; }

    public int CurrentCreditCount { get; private set; }

    public static CreditTracker Create(Guid userId, int maxCreditCount)
    {
        return new CreditTracker()
        {
            UserId = userId,
            MaxCreditCount = maxCreditCount,
            CurrentCreditCount = 0
        };
    }

    public static async Task SetCreditCountAsync(CreditTracker creditTracker, Guid planId, IPlanRepository planRepository, CancellationToken cancellationToken, int multiplier = 1)
    {
        Plan plan = await planRepository.GetByIdAsync(planId, false, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);
        
        Feature feature = plan.Features?.FirstOrDefault(x => x.Code == "MONTHLY_CREDITS")
            ?? throw new ErrorException(PlansDomainErrors.Feature.FeatureNotFound);

        PlanFeature? planFeature = plan.PlanFeatures?.FirstOrDefault(x => x.FeatureId == feature.Id)
            ?? throw new ErrorException(PlansDomainErrors.PlanFeature.PlanFeatureNotFound);

        int value = int.Parse(planFeature.Value) * multiplier;

        creditTracker.MaxCreditCount = value;
        creditTracker.CurrentCreditCount = value;
    }

    public static void ClearCreditCount(CreditTracker creditTracker)
    {
        creditTracker.MaxCreditCount = int.MinValue;
        creditTracker.CurrentCreditCount = 0;
    }

    public static void DecreaseCurrentCreditCount(CreditTracker creditTracker)
    {
        creditTracker.CurrentCreditCount -= 1;
    }
}
