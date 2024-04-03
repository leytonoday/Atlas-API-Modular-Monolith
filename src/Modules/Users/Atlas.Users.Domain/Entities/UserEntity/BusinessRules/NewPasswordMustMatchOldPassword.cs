using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

internal class NewPasswordMustMatchOldPassword(string proposedOldPassword, string actualOldPassword) : IBusinessRule
{
    public string Message => "Invalid credentials";

    public bool IsBroken()
    {
        return proposedOldPassword != actualOldPassword;
    }
}
