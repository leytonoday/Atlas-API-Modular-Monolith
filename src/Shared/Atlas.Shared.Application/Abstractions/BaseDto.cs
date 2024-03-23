namespace Atlas.Shared.Application.Abstractions;

public abstract class BaseDto<TId>
{
    public TId Id { get; set; }
}