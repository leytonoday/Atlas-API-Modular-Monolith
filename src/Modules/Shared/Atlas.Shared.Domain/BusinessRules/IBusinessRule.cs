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
    /// Determines if the business rule has been broken.
    /// </summary>
    /// <returns><c>true</c> if the business rule has been broken, <c>false</c> otherwise.</returns>
    public bool IsBroken();
}
