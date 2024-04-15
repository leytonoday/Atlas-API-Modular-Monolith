using Atlas.Shared.Domain.BusinessRules;
using System.Text.RegularExpressions;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

/// <summary>
/// This business rule checks if the provided password meets the minimum length requirement.
/// </summary>
/// <param name="password">The password to validate against the minimum length requirement.</param>
internal partial class PasswordMustHaveDigitsBusinessRule(string password) : IBusinessRule
{
    /// <inheritdoc/>
    public string Message => "Password must include digits";

    /// <inheritdoc/>
    public string Code => $"User.{nameof(PasswordMustHaveDigitsBusinessRule)}";

    /// <inheritdoc/>
    public bool IsBroken()
    {
        return !HasDigitsRegex().IsMatch(password);
    }

    [GeneratedRegex(@"\d")]
    private static partial Regex HasDigitsRegex();
}
