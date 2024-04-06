using Atlas.Shared.Domain.BusinessRules;
using System.Text.RegularExpressions;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

internal partial class PasswordMustHaveUpperCaseBusinessRule(string password) : IBusinessRule
{
    public string Message => "Password must include uppercase letters";

    public string Code => $"User.{nameof(PasswordMustHaveUpperCaseBusinessRule)}";

    public bool IsBroken()
    {
        return !HasUpperCaseRegex().IsMatch(password);
    }

    [GeneratedRegex(@"[A-Z]")]
    private static partial Regex HasUpperCaseRegex();
}