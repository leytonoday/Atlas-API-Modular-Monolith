namespace Atlas.Law.Infrastructure.Services.PineconeService.Responses;

internal sealed class QueryVectorsResponseMatch
{
    public string Id { get; set; } = null!;

    public float Score { get; set; }
}

internal sealed class QueryVectorsResponse
{
    public IEnumerable<QueryVectorsResponseMatch> Matches { get; set; } = null!;
}
