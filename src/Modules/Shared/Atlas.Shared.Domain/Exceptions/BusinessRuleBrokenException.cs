namespace Atlas.Shared.Domain.Exceptions;

/// <summary>
/// Indicates that a business rule has been broken.
/// </summary>
/// <param name="message">Details of the broken business rule.</param>
/// <param name="code">A code used to identify the rule.</param>
public class BusinessRuleBrokenException(string message, string code) : Exception(message)
{
    public string Code { get; init; } = code;
}
