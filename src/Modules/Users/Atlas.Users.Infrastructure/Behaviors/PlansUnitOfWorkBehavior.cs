using Atlas.Shared.Infrastructure.Behaviors;
using Atlas.Users.Domain;

namespace Atlas.Users.Infrastructure.Behaviors;

public class UsersUnitOfWorkBehavior<TRequest, TResponse>(IUsersUnitOfWork usersUnitOfWork): BaseUnitOfWorkBehavior<TRequest, TResponse, IUsersUnitOfWork>(usersUnitOfWork)
    where TRequest : notnull;