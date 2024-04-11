using System.Text.Json.Serialization;

namespace Atlas.Law.Infrastructure.Services.LargeLanguageModelService.Shared;

/// <summary>
/// Used to represent a message that is sent to the GPT API for completion.
/// </summary>
public class GptCompletionMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;

    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;
}
