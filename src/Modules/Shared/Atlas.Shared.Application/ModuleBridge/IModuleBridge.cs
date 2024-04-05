namespace Atlas.Shared.Application.ModuleBridge;

/// <summary>
/// Used to facillitate synchronous communication betwee modules
/// </summary>
public interface IModuleBridge
{
    public Task SetUserPlanId(Guid userId, Guid? planId, CancellationToken cancellationToken);

    public Task<Guid?> GetUserPlanId(Guid userId, CancellationToken cancellationToken);
}
