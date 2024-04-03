using Atlas.Shared.IntegrationEvents;

namespace Atlas.Shared.Application.Abstractions.Integration;

/// <summary>
/// Represents a handler for <see cref="IIntegrationEvent"/>s that come straight from the <see cref="IEventBus"/>.
/// </summary>
public interface IEventBusIntegrationEventHandler
{
    public Task Handle(IIntegrationEvent @event);
}
