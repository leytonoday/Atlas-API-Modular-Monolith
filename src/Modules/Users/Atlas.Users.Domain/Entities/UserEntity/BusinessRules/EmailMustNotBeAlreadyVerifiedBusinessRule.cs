using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

/// <summary>
/// This business rule checks if a user's email has already been verified.
/// It's intended to be used in scenarios where re-verification is not allowed.
/// </summary>
/// <param name="user">The user to check for pre-existing email verification.</param>
internal class EmailMustNotBeAlreadyVerifiedBusinessRule(User user) : IBusinessRule
{
    /// <inheritdoc/>
    public string Message => "User's email cannot already be confirmed";

    /// <inheritdoc/>
    public string Code => $"User.{nameof(EmailMustNotBeAlreadyVerifiedBusinessRule)}";

    /// <inheritdoc/>
    public bool IsBroken()
    {
        return user.EmailConfirmed == true;
    }
}
