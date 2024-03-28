using Atlas.Shared.Domain.Persistance;

namespace Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity;

public interface IStripeCardFingerprintRepository : IRepository<StripeCardFingerprint>
{
    public Task<StripeCardFingerprint?> GetByFingerprintAsync(string fingerprint, bool trackChanges, CancellationToken cancellationToken);
}