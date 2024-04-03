using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

internal class EmailMustBeVerifiedBusinessRule(User user) : IBusinessRule
{
    public string Message => "Email must be verified";

    public bool IsBroken()
    {
        return user.EmailConfirmed = false;
    }
}
