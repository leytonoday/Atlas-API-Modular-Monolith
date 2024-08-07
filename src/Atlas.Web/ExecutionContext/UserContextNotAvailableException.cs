﻿namespace Atlas.Web.ExecutionContext;

/// <summary>
/// An exception that indicates that the user conntext cannot be accessed.
/// </summary>
public sealed class UserContextNotAvailableException: ApplicationException
{
    public UserContextNotAvailableException() : base("User context is not available within the Execution context.") { }
}
