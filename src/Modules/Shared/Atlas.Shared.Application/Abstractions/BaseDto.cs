namespace Atlas.Shared.Application.Abstractions;

/// <summary>
/// This abstract base class provides a foundation for Data Transfer Objects (DTOs) 
/// with a common identifier property.
/// </summary>
/// <typeparam name="TId">The type of the identifier property. 
/// Can be any type that allows null values.</typeparam>
public abstract class BaseDto<TId>
{
    /// <summary>
    /// Gets or sets the identifier for the DTO. 
    /// </summary>
    public TId? Id { get; set; }
}