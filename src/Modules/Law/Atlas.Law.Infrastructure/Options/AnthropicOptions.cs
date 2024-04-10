namespace Atlas.Law.Infrastructure.Options;

public sealed class AnthropicOptions
{
    public string ApiKey { get; set; } = null!;

    /// <summary>
    /// The model of LLM (Large Language Model) to use for small operations
    /// </summary>
    public string CheapLargeLanguageModelModel { get; set; } = null!;

    /// <summary>
    /// The model of LLM (Large Language Model) to use for large operations where performance is critical
    /// </summary>
    public string ExpensiveLargeLanguageModelModel { get; set; } = null!;
}
