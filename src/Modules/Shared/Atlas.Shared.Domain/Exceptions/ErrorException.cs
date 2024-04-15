using Atlas.Shared.Domain.Errors;

namespace Atlas.Shared.Domain.Exceptions;

/// <summary>
/// Represents an exception that encapsulates one or more errors.
/// </summary>
public class ErrorException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorException"/> class with the specified errors.
    /// </summary>
    /// <param name="errors">The errors to be encapsulated.</param>
    public ErrorException(IEnumerable<Error> errors)
    {
        Errors = errors;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorException"/> class with the specified error.
    /// </summary>
    /// <param name="error">The error to be encapsulated.</param>
    public ErrorException(Error? error)
    {
        Errors = error is not null ? [error] : new List<Error> { Error.UnknownError };
    }

    /// <summary>
    /// Gets or sets the errors encapsulated by this exception.
    /// </summary>
    public IEnumerable<Error>? Errors { get; set; }
}