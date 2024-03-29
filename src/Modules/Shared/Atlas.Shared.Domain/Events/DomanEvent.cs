namespace Atlas.Shared.Domain.Events;

/// <summary>
/// Represents a domain event, which captures something notable that has happened within the domain, but that is a side-effect and shouldn't be handled there and then in the code.
/// </summary>
public abstract record DomainEvent(Guid Id, DateTime OccuredOnUtc) : IDomainEvent
{
    /// <summary>
    /// Protected constructor for creating a domain event with the specified ID and occurrence time.
    /// </summary>
    /// <param name="Id">The unique identifier of the domain event.</param>
    /// <param name="OccuredOnUtc">The timestamp when the domain event occurred in Coordinated Universal Time (UTC).</param>
    protected DomainEvent(Guid Id) : this(Id, DateTime.UtcNow) // TODO - Make this UtcNow actually interact with some IDateTimeProvider service that can get any datetime
    {
    }
}