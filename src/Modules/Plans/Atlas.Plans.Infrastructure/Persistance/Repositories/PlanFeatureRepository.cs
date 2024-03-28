using Atlas.Infrastructure.Persistance;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Plans.Infrastructure.Persistance.Repositories;

internal sealed class PlanFeatureRepository(PlansDatabaseContext context)
    : Repository<PlanFeature, PlansDatabaseContext>(context), IPlanFeatureRepository
{
    public async Task<IEnumerable<PlanFeature>> GetByPlanId(Guid planId, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<PlanFeature> query = GetDbSet(trackChanges);

        return await query.Where(x => x.PlanId == planId).ToListAsync(cancellationToken);
    }
}