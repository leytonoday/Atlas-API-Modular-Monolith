using Atlas.Shared.Domain.BusinessRules;
using PhoneNumbers;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

internal class PhoneNumberMustBeValidBusinessRule(string phoneNumber) : IBusinessRule
{
    public string Message => "The phone number provided is invalid.";

    public bool IsBroken()
    {
        return !PhoneNumberUtil.IsViablePhoneNumber(phoneNumber);
    }
}
