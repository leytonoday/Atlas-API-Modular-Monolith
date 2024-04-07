using System.Text.Json.Serialization;

namespace Atlas.Law.Infrastructure.Services.LargeLanguageModelService.Response;

internal class GptCreateEmbeddingResponseData
{
    [JsonPropertyName("object")]
    public string Object { get; set; } = null!;

    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("embedding")]
    public IEnumerable<float> Embedding { get; set; } = null!;
}

internal class GptCreateEmbeddingResponse
{
    [JsonPropertyName("object")]
    public string Object { get; set; } = null!;

    [JsonPropertyName("data")]
    public IEnumerable<GptCreateEmbeddingResponseData> Data { get; set; } = null!;
}
