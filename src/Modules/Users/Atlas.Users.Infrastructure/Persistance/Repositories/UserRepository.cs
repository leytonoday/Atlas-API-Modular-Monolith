using Atlas.Infrastructure.Persistance;
using Atlas.Users.Domain.Entities.UserEntity;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Users.Infrastructure.Persistance.Repositories;

public sealed class UserRepository(UsersDatabaseContext context)
    : Repository<User, UsersDatabaseContext, Guid>(context), IUserRepository
{
    public async Task<User?> GetByUserNameAsync(string userName, bool trackChanges, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<User> query = GetDbSet(trackChanges);

        return await query.Where(x => x.UserName == userName).FirstOrDefaultAsync(cancellationToken);
    }
}