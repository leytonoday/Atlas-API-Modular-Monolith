using MediatR;

namespace Atlas.Shared.IntegrationEvents;

/// <summary>
/// Represents an integration event, which communicates something that has happened within the system to other systems or services. 
/// In the context of a domain-driven-design monolith, it's an event that indicates something should happen that concerns other sub-domains.
/// </summary>
public interface IIntegrationEvent : INotification
{
    /// <summary>
    /// Gets the unique identifier for the integration event.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the UTC date and time when the event occurred.
    /// </summary>
    public DateTime OccuredOnUtc { get; init; }
}
