using Atlas.Shared.Domain.Entities;

namespace Atlas.Shared.Domain.AggregateRoot;

public class AggregateRoot : Entity, IAggregateRoot
{
}

public class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
{
}