using Atlas.Shared.Domain;
using MediatR;

namespace Atlas.Shared.Infrastructure.Decorators;

/// <summary>
/// A decorator that is applied to <see cref="INotificationHandler{TNotification}"/> that commits any changes that occur within them. Implementations of the <see cref="IRequestHandler{TRequest}"/> or <see cref="IRequestHandler{TRequest, TResponse}"/>
/// interfaces can rely on the <see cref="UnitOfWorkBehavior"/> to commit the changes, because <see cref="INotificationHandler{TNotification}"/> doesn't support MediatR pipelines, so this is an alternative.
/// </summary>
/// <typeparam name="TNotification">The type of notification being handled.</typeparam>
/// <param name="decoratedHandler">The inner notification handler to be decorated.</param>
/// <param name="unitOfWork">The UnitOfWork instance used for managing database transactions.</param>
class NotificationUnitOfWorkDecorator<TNotification>(INotificationHandler<TNotification> decoratedHandler, IUnitOfWork unitOfWork) : IIsDecorator, INotificationHandler<TNotification> where TNotification : class, INotification
{
    /// <summary>
    /// Asynchronously handles a notification request.
    /// </summary>
    /// <param name="notification">The notification to be processed.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        await decoratedHandler.Handle(notification, cancellationToken);

        // If there's been some DB changes, then make the commit
        if (unitOfWork.HasUnsavedChanges())
        {
            await unitOfWork.CommitAsync(cancellationToken);
        }
    }
}