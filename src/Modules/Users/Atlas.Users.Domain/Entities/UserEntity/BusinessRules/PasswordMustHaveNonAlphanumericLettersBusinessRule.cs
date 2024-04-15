using Atlas.Shared.Domain.BusinessRules;
using System.Text.RegularExpressions;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

/// <summary>
/// This business rule checks if the provided password includes at least one non-alphanumeric character.
/// </summary>
/// <param name="password">The password to be checked.</param>
internal partial class PasswordMustHaveNonAlphanumericLettersBusinessRule(string password) : IBusinessRule
{
    /// <inheritdoc/>
    public string Message => "Password must include non-alphanumeric letters";

    /// <inheritdoc/>
    public string Code => $"User.{nameof(PasswordMustHaveNonAlphanumericLettersBusinessRule)}";

    /// <inheritdoc/>
    public bool IsBroken()
    {
        return !HasNonAlphanumericRegex().IsMatch(password);
    }

    [GeneratedRegex(@"\W")]
    private static partial Regex HasNonAlphanumericRegex();
}