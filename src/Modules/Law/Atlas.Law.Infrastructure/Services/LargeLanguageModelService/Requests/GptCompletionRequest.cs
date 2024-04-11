using Atlas.Law.Infrastructure.Services.LargeLanguageModelService.Shared;

namespace Atlas.Law.Infrastructure.Services.LargeLanguageModelService.Requests;

internal class GptCompletionRequestResponseFormat
{
    public string Type { get; set; }
}

internal class GptCompletionRequest
{
    public string Model { get; set; }

    public double Temperature { get; set; }

    public IEnumerable<GptCompletionMessage> Messages { get; set; } = null!;

    public GptCompletionRequestResponseFormat? ResponseFormat { get; set; }
}
