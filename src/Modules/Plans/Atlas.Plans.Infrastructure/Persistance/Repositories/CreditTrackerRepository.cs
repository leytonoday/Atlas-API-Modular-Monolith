using Atlas.Infrastructure.Persistance;
using Atlas.Plans.Domain.Entities.CreditTrackerEntity;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Plans.Infrastructure.Persistance.Repositories;

internal sealed class CreditTrackerRepository(PlansDatabaseContext context)
    : Repository<CreditTracker, PlansDatabaseContext>(context), ICreditTrackerRepository
{
    public async Task<CreditTracker?> GetByUserIdAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<CreditTracker?> query = GetDbSet(trackChanges);

        return await query.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }
}