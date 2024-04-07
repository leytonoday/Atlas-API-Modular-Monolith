namespace Atlas.Law.Infrastructure.Options;

public sealed class AnthropicOptions
{
    public string ApiKey { get; set; } = null!;

    /// <summary>
    /// The specific model of LLM (Large Language Model) to use
    /// </summary>
    public string LargeLanguageModelModel { get; set; } = null!;
}
