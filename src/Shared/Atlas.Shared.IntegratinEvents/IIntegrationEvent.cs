using MediatR;

namespace Atlas.Shared.IntegrationEvents;

public interface IIntegrationEvent : INotification
{
    public Guid Id { get; init; }

    public DateTime OccuredOnUtc { get; init; }
}
