using Atlas.Shared.Domain.Events;
using Atlas.Shared.Domain;
using MediatR;

namespace Atlas.Shared.Application.Abstractions.Messaging;

/// <summary>
/// A wrapper around the <see cref="INotificationHandler{TNotification}"/> from MediatR that ensures idempotence of notification handling for <see cref="IDomainEvent"/>s.
/// Any notification handler that must be idempotent should inherit from <see cref="BaseDomainEventHandler{TEvent, TIUnitOfWork}"/> rather than <see cref="IDomainEventHandler{TEvent}"/>.
/// </summary>
public abstract class BaseDomainEventHandler<TEvent, TIUnitOfWork> : IDomainEventHandler<TEvent>
    where TEvent : IDomainEvent
    where TIUnitOfWork : IUnitOfWork
{
    private readonly TIUnitOfWork _unitOfWork;

    protected BaseDomainEventHandler(TIUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// A wrapper around the actual execution of the domain event handler, which ensures idempotence of domain event execution
    /// </summary>
    /// <param name="notification">The <see cref="IDomainEvent"/> to handle.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        string eventHandlerName = this.GetType().Name;
        if (await _unitOfWork.HasDomainEventBeenHandledAsync(eventHandlerName, notification.Id, cancellationToken))
        {
            return;
        }

        await HandleInner(notification, cancellationToken);

        _unitOfWork.MarkDomainEventAsHandled(eventHandlerName, notification.Id, cancellationToken);
    }

    /// <summary>
    /// Runs the notification handler.
    /// </summary>
    /// <param name="notification">The <see cref="IDomainEvent"/> to handle.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    protected abstract Task HandleInner(TEvent notification, CancellationToken cancellationToken);
};