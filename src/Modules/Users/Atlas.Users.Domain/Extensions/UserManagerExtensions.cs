using Atlas.Users.Domain.Entities.UserEntity;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Domain.Extensions;

public static class UserManagerExtensions
{
    public static Task<User?> FindByUserNameOrEmailAsync(this UserManager<User> userManager, string identifier)
    {
        if (identifier.Contains('@'))
        {
            return userManager.FindByEmailAsync(identifier);
        }

        return userManager.FindByNameAsync(identifier);
    }
}
