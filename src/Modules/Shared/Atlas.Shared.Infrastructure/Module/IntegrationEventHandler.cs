using Atlas.Shared.IntegrationEvents;
using Atlas.Shared.Infrastructure.Integration;
using Atlas.Shared.Domain;
using Microsoft.Extensions.DependencyInjection;
using Atlas.Shared.Application.Abstractions.Integration.Inbox;

namespace Atlas.Shared.Infrastructure.Module;

public class GenericIntegrationEventHandler<TCompositionRoot, TIntegrationEvent> : IIntegrationEventHandler
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
