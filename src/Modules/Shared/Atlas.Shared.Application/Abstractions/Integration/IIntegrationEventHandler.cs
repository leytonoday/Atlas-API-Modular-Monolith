using Atlas.Shared.Domain.Events;
using Atlas.Shared.IntegrationEvents;
using MediatR;

namespace Atlas.Shared.Infrastructure.Integration;

/// <summary>
/// Represents an event handler for a <see cref="IIntegrationEvent"/>.
/// </summary>
/// <typeparam name="TEvent">The type of <see cref="IIntegrationEvent"/> to handle.</typeparam>
public interface IIntegrationEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IIntegrationEvent;

