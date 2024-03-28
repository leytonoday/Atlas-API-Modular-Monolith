using MediatR;

namespace Atlas.Shared.Domain.Events;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}
