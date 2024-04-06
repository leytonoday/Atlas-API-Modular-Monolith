using Atlas.Shared.Domain.BusinessRules;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

internal class OldPasswordMustBeProvidedCorrectlyBusinessRule(string oldPassword, string providedPassword, IPasswordHasher<User> passwordHasher, User user) : IBusinessRule
{
    public string Message => "Invalid credentials";

    public string Code => $"User.{nameof(OldPasswordMustBeProvidedCorrectlyBusinessRule)}";

    public bool IsBroken()
    {
        PasswordVerificationResult comparePassword = passwordHasher.VerifyHashedPassword(user, oldPassword, providedPassword);
        return comparePassword == PasswordVerificationResult.Failed;
    }
}
