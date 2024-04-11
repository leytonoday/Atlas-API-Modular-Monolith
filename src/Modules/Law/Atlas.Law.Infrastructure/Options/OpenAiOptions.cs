namespace Atlas.Law.Infrastructure.Options;

public sealed class OpenAiOptions
{
    public string ApiKey { get; set; } = null!;

    public string ApiUrl { get; set; } = null!;

    public string EmbeddingModel { get; set; } = null!;

    public string Gpt3Model { get; set; } = null!;

    public string Gpt4Model { get; set; } = null!; 
}

