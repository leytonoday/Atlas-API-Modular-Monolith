using Atlas.Shared.Domain.BusinessRules;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

/// <summary>
/// This business rule checks if the provided old password matches the user's hashed password.
/// </summary>
/// <param name="oldPassword">The user's old password.</param>
/// <param name="providedPassword">The password the user provided.</param>
/// <param name="passwordHasher">An interface implementing password hashing logic.</param>
/// <param name="user">The user object containing the hashed password.</param>
internal class OldPasswordMustBeProvidedCorrectlyBusinessRule(string oldPassword, string providedPassword, IPasswordHasher<User> passwordHasher, User user) : IBusinessRule
{
    /// <inheritdoc/>
    public string Message => "Invalid credentials";

    /// <inheritdoc/>
    public string Code => $"User.{nameof(OldPasswordMustBeProvidedCorrectlyBusinessRule)}";

    /// <inheritdoc/>
    public bool IsBroken()
    {
        PasswordVerificationResult comparePassword = passwordHasher.VerifyHashedPassword(user, oldPassword, providedPassword);
        return comparePassword == PasswordVerificationResult.Failed;
    }
}
