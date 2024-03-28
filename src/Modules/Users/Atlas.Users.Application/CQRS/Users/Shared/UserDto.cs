using Atlas.Shared.Application.Abstractions;

namespace Atlas.Users.Application.CQRS.Users.Shared;

public sealed class UserDto : BaseDto<Guid>
{
    public string UserName { get; set; }

    public string Email { get; set; }

    public IEnumerable<string> Roles { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public Guid? PlanId { get; set; }

    public Guid? CustomerId { get; set;}
}