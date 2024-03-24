using Atlas.Infrastructure.Persistance;
using Atlas.Plans.Domain.Entities.FeatureEntity;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Plans.Infrastructure.Persistance.Repositories;

internal sealed class FeatureRepository(PlansDatabaseContext context)
    : Repository<Feature, PlansDatabaseContext, Guid>(context), IFeatureRepository
{
    public async Task<Feature?> GetByNameAsync(string name, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<Feature> query = GetDbSet(trackChanges);

        return await query.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }
}