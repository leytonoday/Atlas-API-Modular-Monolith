namespace Atlas.Law.Infrastructure.Options;

public sealed class OpenAiOptions
{
    public string ApiKey { get; set; } = null!;

    public string ApiUrl { get; set; } = null!;

    public string EmbeddingModel { get; set; } = null!;
}

