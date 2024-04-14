namespace Atlas.Shared.Application.ModuleBridge;

/// <summary>
/// Used to facillitate synchronous communication betwee modules
/// </summary>
public interface IModuleBridge
{
    public Task<Guid?> GetUserPlanId(Guid userId, CancellationToken cancellationToken);

    public Task<bool> DoesUserHaveCredits(Guid userId, CancellationToken cancellationToken);

    public Task DecreaseUserCredits(Guid userId, CancellationToken cancellationToken);
}
