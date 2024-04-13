using Atlas.Infrastructure.Persistance;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;

namespace Atlas.Plans.Infrastructure.Persistance.Repositories;

internal sealed class PlanRepository(PlansDatabaseContext context)
    : Repository<Plan, PlansDatabaseContext, Guid>(context), IPlanRepository
{
    public async Task<bool> IsNameTakenAsync(string name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IQueryable<Plan> query = GetDbSet(false);

        return await query.AnyAsync(x => x.Name == name, cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task<Plan?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IQueryable<Plan> query = GetDbSet(trackChanges);

        // Include the plan features and features.
        return await query
            .Include(x => x.PlanFeatures)
            .Include(x => x.Features)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Plan>> GetByConditionAsync(Expression<Func<Plan, bool>>? condition, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<Plan> query = GetDbSet(trackChanges);
        if (condition != null)
            query = query.Where(condition);

        return await query
            .Include(x => x.PlanFeatures)
            .Include(x => x.Features)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Plan?> GetByInheritsFromIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IQueryable<Plan> query = GetDbSet(trackChanges);

        return await query.FirstOrDefaultAsync(x => x.InheritsFromId == id);
    }

    /// <inheritdoc/>
    public async Task<Plan?> GetByNameAsync(string name, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IQueryable<Plan> query = GetDbSet(trackChanges);

        return await query.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public Task AddPlanFeatureAsync(PlanFeature planFeature, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Context.Set<PlanFeature>().Add(planFeature);
        return Task.CompletedTask;
    }

    public Task RemovePlanFeatureAsync(PlanFeature planFeature, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Context.Set<PlanFeature>().Remove(planFeature);
        return Task.CompletedTask;
    }

    public Task UpdatePlanFeatureAsync(PlanFeature planFeature, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Context.Set<PlanFeature>().Update(planFeature);
        return Task.CompletedTask;
    }

    public Task<PlanFeature?> GetPlanFeatureAsync(Guid planId, Guid featureId, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IQueryable<PlanFeature> query = GetDbSet<PlanFeature>(trackChanges);

        return query.Where(x => x.PlanId == planId && x.FeatureId == featureId).FirstOrDefaultAsync(cancellationToken);
    }


}
