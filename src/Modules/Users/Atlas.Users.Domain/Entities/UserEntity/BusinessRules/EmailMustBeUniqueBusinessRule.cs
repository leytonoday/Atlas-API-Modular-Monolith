using Atlas.Shared.Domain.BusinessRules;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

/// <summary>
/// A business rule ensuring that an email address is unique among users.
/// </summary>
/// <param name="email">The email address to check for uniqueness.</param>
/// <param name="userManager">The user manager used to check for the existence of other users with the same email.</param>
internal class EmailMustBeUniqueBusinessRule(string email, UserManager<User> userManager) : IAsyncBusinessRule
{
    /// <inheritdoc/>
    public string Message => "Email must be unique";

    /// <inheritdoc/>
    public string Code => $"User.{nameof(EmailMustBeUniqueBusinessRule)}";

    /// <inheritdoc/>
    public async Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
    {
        // Check if a user with the specified email already exists.
        User? existing = await userManager.FindByEmailAsync(email);
        return existing is not null;
    }
}
