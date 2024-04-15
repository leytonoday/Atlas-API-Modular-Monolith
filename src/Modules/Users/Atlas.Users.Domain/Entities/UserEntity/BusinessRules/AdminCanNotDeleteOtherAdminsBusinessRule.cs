using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Users.Domain.Entities.UserEntity.BusinessRules;

/// <summary>
/// Administrators are not allowed to delete other administrators, unless they are deleting their own account.
/// </summary>
/// <param name="isDeletingSelf">Is the user making the request the same user that is being requested to be deleted?</param>
/// <param name="isCurrentUserAdmin">Is the user making the request an Admin?</param>
/// <param name="isToBeDeletedUserAdmin">Is the user that is being requested to be deleted an Admin?</param>
internal class AdminCanNotDeleteOtherAdminsBusinessRule(bool isDeletingSelf, bool isCurrentUserAdmin, bool isToBeDeletedUserAdmin) : IBusinessRule
{
    /// <inheritdoc/>
    public string Message => "Administrators can not delete other Administrators.";

    /// <inheritdoc/>
    public string Code => $"User.{nameof(AdminCanNotDeleteOtherAdminsBusinessRule)}";

    /// <inheritdoc/>
    public bool IsBroken()
    {
        return !isDeletingSelf && isCurrentUserAdmin && isToBeDeletedUserAdmin;
    }
}
