using Atlas.Shared.Domain;
using Atlas.Shared.Infrastructure.Persistance;
using Microsoft.Extensions.Logging;

namespace Atlas.Plans.Infrastructure.Persistance;

internal sealed class PlansUnitOfWork(PlansDatabaseContext plansDatabaseContext, ILogger<PlansUnitOfWork> logger) 
    : BaseUnitOfWork<PlansDatabaseContext, ILogger<PlansUnitOfWork>>(plansDatabaseContext, logger),
    IUnitOfWork;