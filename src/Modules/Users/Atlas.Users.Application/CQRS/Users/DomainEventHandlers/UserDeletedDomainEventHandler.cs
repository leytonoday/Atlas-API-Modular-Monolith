using Atlas.Shared.Application.Abstractions.Integration.Outbox;
using Atlas.Shared.Application.Abstractions.Messaging;
using Atlas.Users.Domain.Entities.UserEntity.Events;
using Atlas.Users.IntegrationEvents;
using Microsoft.Extensions.Logging;

namespace Atlas.Users.Application.CQRS.Users.DomainEventHandlers;

internal sealed class UserDeletedDomainEventHandler(IOutboxWriter outboxWriter, ILogger<UserDeletedDomainEventHandler> logger) : IDomainEventHandler<UserDeletedDomainEvent>
{
    public async Task Handle(UserDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("User deleted with Id {Id}, publishing to Outbox", notification.UserId);
        await outboxWriter.WriteAsync(new UserDeletedIntegrationEvent(notification.UserId), cancellationToken);
    }
}
