using Atlas.Shared.IntegrationEvents;

namespace Atlas.Shared.Infrastructure.Integration;

public interface IIntegrationEventHandler
{
    public Task Handle(IIntegrationEvent @event);
}