using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

/// <summary>
/// This business rule checks if a user's email has been verified.
/// </summary>
/// <param name="user">The user to check for email verification.</param>
internal class EmailMustBeVerifiedBusinessRule(User user) : IBusinessRule
{
    /// <inheritdoc/>
    public string Message => "Email must be verified";

    /// <inheritdoc/>
    public string Code => $"User.{nameof(EmailMustBeVerifiedBusinessRule)}";

    /// <inheritdoc/>
    public bool IsBroken()
    {
        return user.EmailConfirmed = false;
    }
}
