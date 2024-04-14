using Atlas.Plans.Application.CQRS.CreditTrackers.Commands.DecreaseUserCredits;
using Atlas.Plans.Application.CQRS.CreditTrackers.Queries.GetDoesUserHaveCredits;
using Atlas.Plans.Infrastructure.Module;
using Atlas.Shared.Application.ModuleBridge;
using Atlas.Users.Application.CQRS.Users.Queries.GetUserById;
using Atlas.Users.Application.CQRS.Users.Shared;
using Atlas.Users.Infrastructure.Module;

namespace Atlas.Web.Modules.Shared;

public class ModuleBridge(IUsersModule usersModule, IPlansModule plansModule) : IModuleBridge
{
    public async Task<Guid?> GetUserPlanId(Guid userId, CancellationToken cancellationToken)
    {
        UserDto userDto = await usersModule.SendQuery(new GetUserByIdQuery(userId), cancellationToken);
        return userDto.PlanId;
    }

    public async Task<bool> DoesUserHaveCredits(Guid userId, CancellationToken cancellationToken)
    {
        return await plansModule.SendQuery(new GetDoesUserHaveCreditsQuery(userId), cancellationToken);
    }

    public async Task DecreaseUserCredits(Guid userId, CancellationToken cancellationToken)
    {
        await plansModule.SendCommand(new DecreaseUserCreditsCommand(userId), cancellationToken);
    }
}
