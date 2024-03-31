using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Abstractions.Integration.Outbox;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;
using Atlas.Users.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler(UserManager<User> userManager, IExecutionContextAccessor executionContext, IOutboxWriter outboxWriter) : ICommandHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User currentUser = await userManager.FindByIdAsync(executionContext.UserId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        User userToBeDeleted = await User.DeleteUserAsync(currentUser, request.UserId, request.Password, userManager);

        await outboxWriter.WriteAsync(new UserDeletedIntegrationEvent(userToBeDeleted.Id), cancellationToken);

        // After the user has been deleted, sign then out manually.
        await userManager.UpdateSecurityStampAsync(userToBeDeleted);
    }
}
