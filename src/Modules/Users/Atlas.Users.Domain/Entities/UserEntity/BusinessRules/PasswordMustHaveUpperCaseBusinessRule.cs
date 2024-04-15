using Atlas.Shared.Domain.BusinessRules;
using System.Text.RegularExpressions;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

/// <summary>
/// This business rule checks if the provided password includes at least one uppercase letter.
/// </summary>
/// <param name="password">The password to be checked.</param>
internal partial class PasswordMustHaveUpperCaseBusinessRule(string password) : IBusinessRule
{
    /// <inheritdoc/>
    public string Message => "Password must include uppercase letters";

    /// <inheritdoc/>
    public string Code => $"User.{nameof(PasswordMustHaveUpperCaseBusinessRule)}";

    /// <inheritdoc/>
    public bool IsBroken()
    {
        return !HasUpperCaseRegex().IsMatch(password);
    }

    [GeneratedRegex(@"[A-Z]")]
    private static partial Regex HasUpperCaseRegex();
}