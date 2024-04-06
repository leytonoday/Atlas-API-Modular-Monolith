namespace Atlas.Shared.IntegrationEvents;

/// <summary>
/// Represents an integration event, which communicates something that has happened within the system to other systems or services. 
/// In the context of a domain-driven-design monolith, it's an event that indicates something should happen that concerns other sub-domains.
/// </summary>
public abstract record IntegrationEvent(Guid Id, DateTime OccuredOnUtc) : IIntegrationEvent
{
    /// <summary>
    /// Protected constructor for creating an integration event with the specified ID and occurrence time.
    /// </summary>
    /// <param name="Id">The unique identifier of the integration event.</param>
    protected IntegrationEvent(Guid Id) : this(Id, DateTime.UtcNow) // TODO - Make this UtcNow actually interact with some IDateTimeProvider service that can get any datetime
    {
    }
}