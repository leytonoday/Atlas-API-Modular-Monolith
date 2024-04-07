namespace Atlas.Law.Infrastructure.Services.LargeLanguageModelService.Requests;

internal class GptCreateEmbeddingRequest
{
    public string Input { get; set; } = null!;

    public string EncodingFormat { get; set; } = null!;

    public string Model { get; set; } = null!;
}

