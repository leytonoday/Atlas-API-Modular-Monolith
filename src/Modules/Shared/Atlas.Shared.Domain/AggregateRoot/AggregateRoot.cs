using Atlas.Shared.Domain.Entities;
using Atlas.Shared.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlas.Shared.Domain.AggregateRoot;

public class AggregateRoot : Entity, IAggregateRoot
{
}

public class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
{
}