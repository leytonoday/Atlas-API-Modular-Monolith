using Atlas.Shared.Domain.Persistance;

namespace Atlas.Users.Domain.Entities.UserEntity;

/// <summary>
/// Represents a repository for managing <see cref="User"/> entities.
/// </summary>
public interface IUserRepository : IRepository<User, Guid>;