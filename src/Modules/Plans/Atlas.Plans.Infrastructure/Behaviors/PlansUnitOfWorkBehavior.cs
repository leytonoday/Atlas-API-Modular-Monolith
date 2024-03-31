using Atlas.Plans.Domain;
using Atlas.Shared.Infrastructure.Behaviors;

namespace Atlas.Plans.Infrastructure.Behaviors;

public class PlansUnitOfWorkBehavior<TRequest, TResponse>(IPlansUnitOfWork plansUnitOfWork): BaseUnitOfWorkBehavior<TRequest, TResponse, IPlansUnitOfWork>(plansUnitOfWork)
    where TRequest : notnull;