using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

/// <summary>
/// This business rule checks if the provided password meets the minimum length requirement.
/// </summary>
/// <param name="password">The password to check.</param>
internal partial class PasswordMustBeMinimumLengthBusinessRule(string password) : IBusinessRule
{
    private const int MinimumLength = 10; // Change this value according to your requirements

    /// <inheritdoc/>
    public string Message => $"Password must be at least {MinimumLength} characters long";

    /// <inheritdoc/>
    public string Code => $"User.{nameof(PasswordMustBeMinimumLengthBusinessRule)}";

    /// <inheritdoc/>
    public bool IsBroken()
    {
        return password.Length < MinimumLength;
    }
}
