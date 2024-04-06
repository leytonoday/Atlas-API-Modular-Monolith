using Atlas.Shared.Domain.BusinessRules;
using System.Text.RegularExpressions;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

internal partial class PasswordMustHaveNonAlphanumericLettersBusinessRule(string password) : IBusinessRule
{
    public string Message => "Password must include non-alphanumeric letters";

    public string Code => $"User.{nameof(PasswordMustHaveNonAlphanumericLettersBusinessRule)}";

    public bool IsBroken()
    {
        return !HasNonAlphanumericRegex().IsMatch(password);
    }

    [GeneratedRegex(@"\W")]
    private static partial Regex HasNonAlphanumericRegex();
}