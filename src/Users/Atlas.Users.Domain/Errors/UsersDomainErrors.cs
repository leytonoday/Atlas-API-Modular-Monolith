﻿using Atlas.Shared.Domain.Errors;

namespace Atlas.Users.Domain.Errors;

public static class UsersDomainErrors
{
    /// <summary>
    /// Creates a standard error code for the <see cref="Error"/> class by combining the name of the class and the name of the property separated by a dot. e.g., User.EmailAlreadyInUse
    /// </summary>
    /// <param name="type">The type on which the error has occured.</param>
    /// <param name="propertyName">The name of the property that calls the method.</param>
    /// <returns>An error code for the <see cref="Error"/> class.</returns>
    private static string CreateErrorCode(Type type, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
    {
        return $"{type.Name}.{propertyName}";
    }

    public static class Authentication
    {
        public static readonly Error NotAuthenticated = new(CreateErrorCode(typeof(Authentication)), "The user cannot be signed out as they are not signed in.");
    }

    public static class Email
    {
        public static readonly Error CannotBeEmpty = new(CreateErrorCode(typeof(Email)), "The email for the provided cannot be empty.");
    }

    public static class PhoneNumber
    {
        public static readonly Error Invalid = new(CreateErrorCode(typeof(PhoneNumber)), "The phone number provided is invalid.");
    }

    public static class User
    {
        public static readonly Error EmailAlreadyInUse = new(CreateErrorCode(typeof(User)), "The specified email is already in use.");

        public static readonly Error UserNameAlreadyInUse = new(CreateErrorCode(typeof(User)), "The specified username is already in use.");

        public static readonly Error InvalidCredentials = new(CreateErrorCode(typeof(User)), "The provided credentials are invalid.");

        public static readonly Error MustVerifyEmail = new(CreateErrorCode(typeof(User)), "Email must be verified before signing in.");

        public static readonly Error UserNotFound = new(CreateErrorCode(typeof(User)), "The User could not be found.");

        public static readonly Error EmailAlreadyVerified = new(CreateErrorCode(typeof(User)), "The email for the provided user is already verified.");

        public static readonly Error NonAdminCanOnlyDeleteOwnAccount = new(CreateErrorCode(typeof(User)), "Non-Administrators can only delete their own accounts.");

        public static readonly Error AdminCanNotDeleteOtherAdmins = new(CreateErrorCode(typeof(User)), "Administrators can not delete other Administrators.");

        public static readonly Error InvalidToken = new(CreateErrorCode(typeof(User)), "The token provided is invalid.");
    }
}


