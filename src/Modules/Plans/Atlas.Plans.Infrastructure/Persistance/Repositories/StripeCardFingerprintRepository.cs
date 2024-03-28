using Atlas.Infrastructure.Persistance;
using Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Plans.Infrastructure.Persistance.Repositories;

internal sealed class StripeCardFingerprintRepository(PlansDatabaseContext context)
    : Repository<StripeCardFingerprint, PlansDatabaseContext>(context), IStripeCardFingerprintRepository
{
    public async Task<StripeCardFingerprint?> GetByFingerprintAsync(string fingerprint, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IQueryable<StripeCardFingerprint> query = GetDbSet(trackChanges);

        return await query.FirstOrDefaultAsync(x => x.Fingerprint == fingerprint, cancellationToken);
    }
}