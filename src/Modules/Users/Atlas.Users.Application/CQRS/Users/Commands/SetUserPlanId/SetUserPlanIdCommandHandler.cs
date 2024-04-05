using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Users.Domain.Entities.UserEntity;
using Atlas.Users.Domain.Errors;

namespace Atlas.Users.Application.CQRS.Users.Commands.SetUserPlanId;

internal sealed class SetUserPlanIdCommandHandler(IUserRepository userRepository) : ICommandHandler<SetUserPlanIdCommand>
{
    public async Task Handle(SetUserPlanIdCommand request, CancellationToken cancellationToken)
    {
        User user = await userRepository.GetByIdAsync(request.UserId, true, cancellationToken)
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        User.SetPlanId(user, request.PlanId);

        await userRepository.UpdateAsync(user, cancellationToken);
    }
}
