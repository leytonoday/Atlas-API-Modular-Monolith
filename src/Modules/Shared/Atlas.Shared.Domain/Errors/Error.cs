namespace Atlas.Shared.Domain.Errors;

using System;

/// <summary>
/// Represents an error with a code and a message.
/// </summary>
public class Error : IEquatable<Error>
{
    /// <summary>
    /// Gets an unknown error.
    /// </summary>
    public static readonly Error UnknownError = new(nameof(UnknownError), "An unknown error has occurred.");

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with the specified code and message.
    /// </summary>
    /// <param name="code">The code of the error.</param>
    /// <param name="message">The message of the error.</param>
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// Gets the message of the error.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the code of the error.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Determines whether the specified <see cref="Error"/> is equal to the current <see cref="Error"/>.
    /// </summary>
    /// <param name="other">The error to compare with the current error.</param>
    /// <returns><c>true</c> if the specified error is equal to the current error; otherwise, <c>false</c>.</returns>
    public bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Message == other.Message && Code == other.Code;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Error"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current error.</param>
    /// <returns><c>true</c> if the specified object is equal to the current error; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is Error error && Equals(error);
    }

    /// <summary>
    /// Returns the hash code for this error.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => HashCode.Combine(Code, Message);
}
