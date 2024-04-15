using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

/// <summary>
/// This business rule checks if a username adheres to the allowed character set.
/// </summary>
/// <param name="password">The password to be checked.</param>
internal partial class UserNameMustUseAllowedCharactersBusinessRule(string userName) : IBusinessRule
{
    private const string AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    /// <inheritdoc/>
    public string Message => "UserName must use only allowed characters: " + AllowedCharacters;

    /// <inheritdoc/>
    public string Code => $"User.{nameof(UserNameMustUseAllowedCharactersBusinessRule)}";

    /// <inheritdoc/>
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