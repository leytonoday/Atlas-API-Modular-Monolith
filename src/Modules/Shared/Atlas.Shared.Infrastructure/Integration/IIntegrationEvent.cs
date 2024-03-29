using MediatR;

namespace Atlas.Shared.Infrastructure.Integration;

public interface IIntegrationEvent : INotification
{
    public Guid Id { get; init; }

    public DateTime OccuredOnUtc { get; init; }
}
