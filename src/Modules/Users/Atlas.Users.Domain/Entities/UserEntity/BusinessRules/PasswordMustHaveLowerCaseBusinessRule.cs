using Atlas.Shared.Domain.BusinessRules;
using System.Text.RegularExpressions;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

internal partial class PasswordMustHaveLowerCaseBusinessRule(string password) : IBusinessRule
{
    public string Message => "Password must include lowercase letters";

    public string Code => $"User.{nameof(PasswordMustHaveLowerCaseBusinessRule)}";

    public bool IsBroken()
    {
        return !HasLowerCaseRegex().IsMatch(password);
    }

    [GeneratedRegex(@"[a-z]")]
    private static partial Regex HasLowerCaseRegex();
}