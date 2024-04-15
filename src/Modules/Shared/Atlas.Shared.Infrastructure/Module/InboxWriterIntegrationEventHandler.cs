using Atlas.Shared.IntegrationEvents;
using Atlas.Shared.Domain;
using Atlas.Shared.Application.Abstractions.Integration.Inbox;
using Atlas.Shared.Application.Abstractions.Integration;
using Autofac;

namespace Atlas.Shared.Infrastructure.Module;

/// <summary>
/// A handler for integration events that come straight from the <see cref="IEventBus"/>. It converts them to Inbox messages.
/// adds them to the modules Inbox.
/// </summary>
/// <typeparam name="TCompositionRoot">The type of the composition root used for dependency resolution.</typeparam>
/// <typeparam name="TIntegrationEvent">The type of integration event this handler can process.</typeparam>
public class InboxWriterIntegrationEventHandler<TCompositionRoot, TIntegrationEvent> : IEventBusIntegrationEventHandler
    where TCompositionRoot : ICompositionRoot
    where TIntegrationEvent : IIntegrationEvent
{
    /// <inheritdoc cref="IEventBusIntegrationEventHandler.Handle(IIntegrationEvent)"/>
    public async Task Handle(IIntegrationEvent integrationEvent)
    {
        using var scope = TCompositionRoot.BeginLifetimeScope();

        var unitOfWork = scope.Resolve<IUnitOfWork>();
        var inbox = scope.Resolve<IInboxWriter>();

        await inbox.WriteAsync(integrationEvent, default);

        await unitOfWork.CommitAsync();
    }
}
