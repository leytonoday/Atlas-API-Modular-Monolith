﻿using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Shared.Domain.Errors;

namespace Atlas.Law.Domain.Errors;

public static class LawDomainErrors
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

    public static class Law
    {
        public static readonly Error LegalDocumentNotFound = new(CreateErrorCode(typeof(LegalDocument)), "The LegalDocument was not found.");
    }
}