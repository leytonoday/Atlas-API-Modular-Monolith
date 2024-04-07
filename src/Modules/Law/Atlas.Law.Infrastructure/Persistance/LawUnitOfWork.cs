using Atlas.Shared.Domain;
using Atlas.Shared.Infrastructure.Persistance;
using Microsoft.Extensions.Logging;

namespace Atlas.Law.Infrastructure.Persistance;

internal sealed class LawUnitOfWork(LawDatabaseContext lawDatabaseContext, ILogger<LawUnitOfWork> logger)
    : BaseUnitOfWork<LawDatabaseContext, ILogger<LawUnitOfWork>>(lawDatabaseContext, logger),
    IUnitOfWork;