using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

/// <summary>
/// Non-administrators should only be able to delete their own accounts.
/// </summary>
/// <param name="isDeletingSelf">Is the user making the request the same user that is being requested to be deleted?</param>
/// <param name="isCurrentUserAdmin">Is the user making the request an Admin?</param>
internal class NonAdminCanOnlyDeleteOwnAccountBusinessRule(bool isDeletingSelf, bool isCurrentUserAdmin) : IBusinessRule
{
    public string Message => "Non-Administrators can only delete their own accounts.";

    public string Code => $"User.{nameof(NonAdminCanOnlyDeleteOwnAccountBusinessRule)}";

    public bool IsBroken()
    {
        return !isCurrentUserAdmin && !isDeletingSelf;
    }
}
