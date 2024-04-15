using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

/// <summary>
/// This business rule checks if a username already exists in the system.
/// </summary>
/// <param name="userName">The username to check.</param>
/// <param name="userRepository">The user repository used to check for other users.</param>
internal class UserNameMustBeUniqueBusinessRule(string userName, IUserRepository userRepository) : IAsyncBusinessRule
{
    /// <inheritdoc/>
    public string Message => "UserName must be unique";

    /// <inheritdoc/>
    public string Code => $"User.{nameof(UserNameMustBeUniqueBusinessRule)}";

    /// <inheritdoc/>
    public async Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
    {
        User? existing = await userRepository.GetByUserNameAsync(userName, false, cancellationToken);
        return existing is not null;
    }
}
