using Atlas.Shared.Application.ModuleBridge;
using Atlas.Users.Application.CQRS.Users.Commands.SetUserPlanId;
using Atlas.Users.Application.CQRS.Users.Queries.GetUserById;
using Atlas.Users.Application.CQRS.Users.Shared;
using Atlas.Users.Infrastructure.Module;

namespace Atlas.Web.Modules.Shared;

public class ModuleBridge(IUsersModule usersModule) : IModuleBridge
{
    public async Task<Guid?> GetUserPlanId(Guid userId, CancellationToken cancellationToken)
    {
        UserDto userDto = await usersModule.SendQuery(new GetUserByIdQuery(userId), cancellationToken);
        return userDto.PlanId;
    }

    public async Task SetUserPlanId(Guid userId, Guid? planId, CancellationToken cancellationToken)
    {
        await usersModule.SendCommand(new SetUserPlanIdCommand(userId, planId), cancellationToken);
    }
}
