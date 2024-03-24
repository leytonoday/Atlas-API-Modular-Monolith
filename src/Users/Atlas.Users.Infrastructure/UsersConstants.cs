namespace Atlas.Users.Infrastructure;

internal static class UsersConstants
{
    internal static class Identity
    {
        internal const string AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    }

    internal static class SeedData
    {
        internal static readonly Guid SupportUserId = new("ee845b42-3b09-48b1-98ae-8809693b8e46");

        internal static readonly string SupportUserEmail = "support@atlas.com";

        internal static readonly string SupportUserUserName = "support";

        internal static readonly Guid AdministratorRoleId = new("9092750c-632d-4d43-9d12-fa16282a9524");
    }


    /// <summary>
    /// Provides constants for table names used in the database for the users sub-domain.
    /// </summary>
    internal static class TableNames
    {
        /// <summary>
        /// Represents the table name for <see cref="User"/> entities in the database.
        /// </summary>
        internal const string Users = nameof(Users);

        /// <summary>
        /// Represents the table name for <see cref="IdentityUserRole"/> entities in the database.
        /// </summary>
        internal const string UserRoles = nameof(UserRoles);
    }
}
