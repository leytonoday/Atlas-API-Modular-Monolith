using Atlas.Shared.Domain;
using MediatR;

namespace Atlas.Shared.Infrastructure.Decorators;

/// <summary>
/// A decorator that is applied to <see cref="INotificationHandler{TNotification}"/> that commits any changes that occur within them. Implementations of the <see cref="IRequestHandler{TRequest}"/> or <see cref="IRequestHandler{TRequest, TResponse}"/>
/// interfaces can rely on the <see cref="UnitOfWorkBehavior"/> to commit the changes, but <see cref="INotificationHandler{TNotification}"/> doesn't support MediatR pipelines, so this is an alternative.
/// </summary>
/// <typeparam name="TNotification"></typeparam>
/// <param name="decoratedHandler"></param>
/// <param name="unitOfWork"></param>
class NotificationUnitOfWorkDecorator<TNotification>(INotificationHandler<TNotification> decoratedHandler, IUnitOfWork unitOfWork) : IIsDecorator, INotificationHandler<TNotification> where TNotification : class, INotification
{
    public async Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        await decoratedHandler.Handle(notification, cancellationToken);

        if (unitOfWork.HasUnsavedChanges())
        {
            await unitOfWork.CommitAsync(cancellationToken);
        }
    }
}