using Atlas.Shared.Domain.BusinessRules;
using PhoneNumbers;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

/// <summary>
/// This business rule checks if the provided phone number is valid using a phone number validation library.
/// </summary>
/// <param name="phoneNumber">The phone number to check.</param>
internal class PhoneNumberMustBeValidBusinessRule(string phoneNumber) : IBusinessRule
{
    /// <inheritdoc/>
    public string Message => "The phone number provided is invalid.";

    /// <inheritdoc/>
    public string Code => $"User.{nameof(PhoneNumberMustBeValidBusinessRule)}";

    /// <inheritdoc/>
    public bool IsBroken()
    {
        return !PhoneNumberUtil.IsViablePhoneNumber(phoneNumber);
    }
}
