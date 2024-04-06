using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

internal class UserNameMustBeUniqueBusinessRule(string userName, IUserRepository userRepository) : IAsyncBusinessRule
{
    public string Message => "UserName must be unique";

    public string ErrorCode => $"User.{nameof(UserNameMustBeUniqueBusinessRule)}";

    public async Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
    {
        User? existing = await userRepository.GetByUserNameAsync(userName, false, cancellationToken);
        return existing is not null;
    }
}
