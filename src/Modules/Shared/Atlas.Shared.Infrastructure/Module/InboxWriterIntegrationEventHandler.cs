using Atlas.Shared.IntegrationEvents;
using Atlas.Shared.Domain;
using Microsoft.Extensions.DependencyInjection;
using Atlas.Shared.Application.Abstractions.Integration.Inbox;
using Atlas.Shared.Application.Abstractions.Integration;

namespace Atlas.Shared.Infrastructure.Module;

/// <summary>
/// A handler for integration events that come straight from the <see cref="IEventBus"/>. It converts them to Inbox messages.
/// adds them 
/// </summary>
/// <typeparam name="TCompositionRoot"></typeparam>
/// <typeparam name="TIntegrationEvent"></typeparam>
public class InboxWriterIntegrationEventHandler<TCompositionRoot, TIntegrationEvent> : IEventBusIntegrationEventHandler
    where TCompositionRoot : ICompositionRoot
    where TIntegrationEvent : IIntegrationEvent
{
    public async Task Handle(IIntegrationEvent integrationEvent)
    {
        using var scope = TCompositionRoot.BeginLifetimeScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var inbox = scope.ServiceProvider.GetRequiredService<IInboxWriter>();

        await inbox.WriteAsync(integrationEvent, default);

        await unitOfWork.CommitAsync();
    }
}
