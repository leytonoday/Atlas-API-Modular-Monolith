using Atlas.Shared.Domain.Events;
using MediatR;

namespace Atlas.Shared.Application.Abstractions.Messaging;

/// <summary>
/// Represents an event handler for a <see cref="IDomainEvent"/>.
/// </summary>
/// <typeparam name="TEvent">The type of <see cref="IDomainEvent"/> to handle.</typeparam>
public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent> 
    where TEvent : IDomainEvent;
