using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

internal class EmailMustNotBeAlreadyVerifiedBusinessRule(User user) : IBusinessRule
{
    public string Message => "User's email cannot already be confirmed";

    public string ErrorCode => $"User.{nameof(EmailMustNotBeAlreadyVerifiedBusinessRule)}";

    public bool IsBroken()
    {
        return user.EmailConfirmed == true;
    }
}
