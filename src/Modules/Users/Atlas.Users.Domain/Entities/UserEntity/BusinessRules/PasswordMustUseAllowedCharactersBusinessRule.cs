using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

internal partial class UserNameMustUseAllowedCharactersBusinessRule(string userName) : IBusinessRule
{
    private const string AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public string Message => "UserName must use only allowed characters: " + AllowedCharacters;

    public string ErrorCode => $"User.{nameof(UserNameMustUseAllowedCharactersBusinessRule)}";

    public bool IsBroken()
    {
        return !IsUsingAllowedCharacters();
    }

    private bool IsUsingAllowedCharacters()
    {
        foreach (char c in userName)
        {
            if (!AllowedCharacters.Contains(c))
            {
                return false;
            }
        }
        return true;
    }
}