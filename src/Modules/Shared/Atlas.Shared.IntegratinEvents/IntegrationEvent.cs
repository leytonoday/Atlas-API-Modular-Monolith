namespace Atlas.Shared.IntegrationEvents;

/// <inheritdoc cref="IIntegrationEvent"/>
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