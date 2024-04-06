namespace Atlas.Shared.Domain.Exceptions;

/// <summary>
/// Indicates that a business rule has been broken.
/// </summary>
/// <param name="message">Details of the broken business rule.</param>
/// <param name="errorCode">A code used to identify the error.</param>
public class BusinessRuleBrokenException(string message, string errorCode) : Exception(message)
{
    public string ErrorCode { get; init; } = errorCode;
}
