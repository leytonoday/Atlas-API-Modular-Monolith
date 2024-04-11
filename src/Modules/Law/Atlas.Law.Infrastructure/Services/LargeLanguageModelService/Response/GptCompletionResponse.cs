using Atlas.Law.Infrastructure.Services.LargeLanguageModelService.Shared;
using System.Text.Json.Serialization;

namespace Atlas.Law.Infrastructure.Services.LargeLanguageModelService.Response;

public class GptComplationResponseChoice
{
    [JsonPropertyName("message")]
    public GptCompletionMessage Message { get; set; } = null!;
}

public class GptCompletionResponse
{
    [JsonPropertyName("choices")]
    public IEnumerable<GptComplationResponseChoice> Choices { get; set; } = null!;
}
