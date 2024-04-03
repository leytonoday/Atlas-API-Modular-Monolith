using Atlas.Shared.Domain.BusinessRules;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

internal class UserNameMustBeUniqueBusinessRule(string userName, UserManager<User> userManager) : IAsyncBusinessRule
{
    public string Message => "UserName must be unique";

    public async Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
    {
        User? existing = await userManager.FindByNameAsync(userName);
        return existing is not null;
    }
}
