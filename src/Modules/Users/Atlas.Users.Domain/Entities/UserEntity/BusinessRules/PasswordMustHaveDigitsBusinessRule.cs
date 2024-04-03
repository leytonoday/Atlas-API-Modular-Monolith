﻿using Atlas.Shared.Domain.BusinessRules;
using System.Text.RegularExpressions;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

internal partial class PasswordMustHaveDigitsBusinessRule(string password) : IBusinessRule
{
    public string Message => "Password must include digits";

    public bool IsBroken()
    {
        return !HasDigitsRegex().IsMatch(password);
    }

    [GeneratedRegex(@"\d")]
    private static partial Regex HasDigitsRegex();
}