using Atlas.Shared.Domain.BusinessRules;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

internal class EmailMustBeUniqueBusinessRule(string email, UserManager<User> userManager) : IAsyncBusinessRule
{
    public string Message => "Email must be unique";

    public async Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
    {
        User? existing = await userManager.FindByEmailAsync(email);
        return existing is not null;
    }
}
