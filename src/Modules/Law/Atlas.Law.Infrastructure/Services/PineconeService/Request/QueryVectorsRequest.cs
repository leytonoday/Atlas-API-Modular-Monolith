namespace Atlas.Law.Infrastructure.Services.PineconeService.Request;

internal class QueryVectorsRequest
{
    public int TopK { get; set; }

    public IEnumerable<float> Vector { get; set; } = null!;
}
