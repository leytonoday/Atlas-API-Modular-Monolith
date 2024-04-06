namespace Atlas.Shared.Domain.BusinessRules;

/// <summary>
/// Represents a business rule that the application must conform to.
/// </summary>
public interface IBusinessRule
{
    /// <summary>
    /// Details of the business rule that was broken.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// A unique error code associated with the business rule violation.
    /// This can be used for identification and categorization of errors.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Determines if the business rule has been broken.
    /// </summary>
    /// <returns><c>true</c> if the business rule has been broken, <c>false</c> otherwise.</returns>
    public bool IsBroken();
}
