using MediatR;

namespace Atlas.Shared.Domain.Events;

/// <summary>
/// Represents a domain event, which captures something notable that has happened within the domain, but that is a side-effect and shouldn't be handled there and then in the code.
/// </summary>
public interface IDomainEvent : INotification;
