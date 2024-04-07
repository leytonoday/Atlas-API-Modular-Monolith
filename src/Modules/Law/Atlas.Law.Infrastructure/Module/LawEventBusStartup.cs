using Atlas.Shared.Infrastructure.Integration.Bus;
using Atlas.Shared.Infrastructure.Module;
using Atlas.Shared.IntegrationEvents;
using Microsoft.Extensions.Logging;

namespace Atlas.Law.Infrastructure.Module;

internal class LawEventBusStartup : IEventBusStartup
{
    public static void Initialize(ILogger logger, IEventBus eventBus)
    {
        SubscribeToIntegrationEvents(logger, eventBus);
    }

    public static void SubscribeToIntegrationEvents(ILogger logger, IEventBus eventBus)
    {
    }

    private static void SubscribeToIntegrationEvent<TIntegrationEvent>(ILogger logger, IEventBus eventBus)
        where TIntegrationEvent : IIntegrationEvent
    {
        logger.LogInformation("Subscribe to {@IntegrationEvent}", typeof(TIntegrationEvent).FullName);
        eventBus.Subscribe<TIntegrationEvent>(new InboxWriterIntegrationEventHandler<LawCompositionRoot, TIntegrationEvent>());
    }
}