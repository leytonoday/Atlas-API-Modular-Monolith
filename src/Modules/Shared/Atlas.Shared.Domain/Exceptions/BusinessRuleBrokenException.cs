namespace Atlas.Shared.Domain.Exceptions;

/// <summary>
/// Indicates that a business rule has been broken.
/// </summary>
/// <param name="message">Details of the broken business rule.</param>
public class BusinessRuleBrokenException(string message) : Exception(message);
