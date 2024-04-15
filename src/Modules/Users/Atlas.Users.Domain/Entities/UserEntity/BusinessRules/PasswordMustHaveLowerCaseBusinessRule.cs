using Atlas.Shared.Domain.BusinessRules;
using System.Text.RegularExpressions;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

/// <summary>
/// A business rule ensuring that a password contains at least one lowercase letter.
/// </summary>
/// <param name="password">The password to be checked.</param>
internal partial class PasswordMustHaveLowerCaseBusinessRule(string password) : IBusinessRule
{
    /// <inheritdoc/>
    public string Message => "Password must include lowercase letters";

    /// <inheritdoc/>
    public string Code => $"User.{nameof(PasswordMustHaveLowerCaseBusinessRule)}";

    /// <inheritdoc/>
    public bool IsBroken()
    {
        return !HasLowerCaseRegex().IsMatch(password);
    }

    [GeneratedRegex(@"[a-z]")]
    private static partial Regex HasLowerCaseRegex();
}