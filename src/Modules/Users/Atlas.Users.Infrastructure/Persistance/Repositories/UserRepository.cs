using Atlas.Infrastructure.Persistance;
using Atlas.Users.Domain.Entities.UserEntity;

namespace Atlas.Users.Infrastructure.Persistance.Repositories;

public sealed class UserRepository(UsersDatabaseContext context)
    : Repository<User, UsersDatabaseContext, Guid>(context), IUserRepository;