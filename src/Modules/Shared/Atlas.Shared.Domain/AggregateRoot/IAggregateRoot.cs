namespace Atlas.Shared.Domain.AggregateRoot;

/// <summary>
/// Marker interface for aggregate roots in Domain-Driven Design (DDD).
/// </summary>
/// <remarks>
/// An aggregate is a cluster of associated objects that are treated as a unit for the purpose of data changes.
/// The aggregate root is the primary access point to the aggregate. It protects the aggregate integrity by
/// enforcing consistency rules for all objects within the aggregate. In DDD, aggregates help to define
/// transactional boundaries and encapsulate business logic.
/// </remarks>
public interface IAggregateRoot;