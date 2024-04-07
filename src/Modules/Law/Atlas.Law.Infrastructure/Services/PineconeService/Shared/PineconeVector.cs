namespace Atlas.Law.Infrastructure.Services.PineconeService.Shared;

internal sealed class PineconeVector
{
    public string Id { get; set; } = null!;

    public IEnumerable<float> Values { get; set; } = null!;
}
