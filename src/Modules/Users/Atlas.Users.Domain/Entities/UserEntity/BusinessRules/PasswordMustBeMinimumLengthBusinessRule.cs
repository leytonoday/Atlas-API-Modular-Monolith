using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

internal partial class PasswordMustBeMinimumLengthBusinessRule(string password) : IBusinessRule
{
    private const int MinimumLength = 10; // Change this value according to your requirements

    public string Message => $"Password must be at least {MinimumLength} characters long";

    public bool IsBroken()
    {
        return password.Length < MinimumLength;
    }
}
