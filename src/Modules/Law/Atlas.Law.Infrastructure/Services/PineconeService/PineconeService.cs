using Atlas.Law.Application.Services;
using Atlas.Law.Infrastructure.Options;
using Atlas.Law.Infrastructure.Services.PineconeService.Request;
using Atlas.Law.Infrastructure.Services.PineconeService.Responses;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Atlas.Law.Infrastructure.Services.PineconeService;

/// <summary>
/// Service class for interacting with the Pinecone API and implementing the IVectorDatabaseService interface.
/// </summary>
internal class PineconeService : BaseApiService, IVectorDatabaseService
{
    private readonly PineconeOptions _pineconeOptions;

    private readonly JsonSerializerOptions _jsonSerialisationOtions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public PineconeService(IOptions<PineconeOptions> options) : base(null)
    {
        HttpClient.DefaultRequestHeaders.Add("Api-Key", options.Value.ApiKey);
        _pineconeOptions = options.Value;
    }

    /// <inheritdoc/>
    public Task InsertVectorAsync(IEnumerable<float> vector, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetSimilarVectorIdsAsync(IEnumerable<float> vector, CancellationToken cancellationToken)
    {
        QueryVectorsResponse result = await PostAsync<QueryVectorsResponse, QueryVectorsRequest>("/query", new QueryVectorsRequest
        {
            TopK = 1,
            Vector = vector,
        }, cancellationToken, _pineconeOptions.LegalDocumentIndexUrl);

        return result.Matches.Select(x => x.Id).ToList();
    }

    protected override string SerializeToJson<TBody>(TBody body)
    {
        // Serialize the object using snake_case
        return JsonSerializer.Serialize(body, _jsonSerialisationOtions);
    }

    protected override TResponse DeserialzeFromJson<TResponse>(string body)
    {
        return JsonSerializer.Deserialize<TResponse>(body, _jsonSerialisationOtions)!;
    }
}
