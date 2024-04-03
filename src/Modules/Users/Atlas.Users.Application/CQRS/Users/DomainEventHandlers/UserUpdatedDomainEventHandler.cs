using Atlas.Shared.Application.Abstractions.Integration.Outbox;
using Atlas.Shared.Application.Abstractions.Messaging;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Entities.UserEntity.Events;
using Atlas.Users.Domain.Errors;
using Atlas.Users.IntegrationEvents;
using Microsoft.Extensions.Logging;

namespace Atlas.Users.Application.CQRS.Users.DomainEventHandlers;

internal sealed class UserUpdatedDomainEventHandler(IUserRepository userRepository, IOutboxWriter outboxWriter, ILogger<UserUpdatedDomainEventHandler> logger) : IDomainEventHandler<UserUpdatedDomainEvent>
{
    public async Task Handle(UserUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("User updated with Id {Id}, publishing to Outbox", notification.UserId);
        User user = await userRepository.GetByIdAsync(notification.UserId, false, cancellationToken)
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        await outboxWriter.WriteAsync(new UserUpdatedIntegrationEvent(user.Id, user.UserName, user.Email, user.PhoneNumber), cancellationToken);
    }
}
