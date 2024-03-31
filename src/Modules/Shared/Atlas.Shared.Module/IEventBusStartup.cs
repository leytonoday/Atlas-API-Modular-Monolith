using Atlas.Shared.Infrastructure.Integration.Bus;
using Microsoft.Extensions.Logging;

namespace Atlas.Shared.Module;

public interface IEventBusStartup
{
    public static abstract void Initialize(ILogger logger, IEventBus eventBus);

    public static abstract void SubscribeToIntegrationEvents(ILogger logger, IEventBus eventBus);
}
