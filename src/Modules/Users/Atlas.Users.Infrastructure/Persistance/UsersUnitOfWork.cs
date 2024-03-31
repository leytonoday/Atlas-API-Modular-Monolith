using Atlas.Shared.Domain;
using Atlas.Shared.Infrastructure.Persistance;
using Atlas.Users.Domain;
using Microsoft.Extensions.Logging;

namespace Atlas.Users.Infrastructure.Persistance;

internal sealed class UsersUnitOfWork(UsersDatabaseContext usersDatabaseContext, ILogger<UsersUnitOfWork> logger) :
    BaseUnitOfWork<UsersDatabaseContext, ILogger<UsersUnitOfWork>>(usersDatabaseContext, logger),
    IUnitOfWork;