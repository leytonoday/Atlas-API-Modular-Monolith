using Atlas.Law.Application.Services;
using Atlas.Law.Domain.Entities.EurLexSumDocumentEntity;
using Atlas.Law.Infrastructure.Options;
using Atlas.Law.Infrastructure.Services.LargeLanguageModelService.Requests;
using Atlas.Law.Infrastructure.Services.LargeLanguageModelService.Response;
using Claudia;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Atlas.Law.Infrastructure.Services.LargeLanguageModelService;

internal class LargeLanguageModelService : BaseApiService, ILargeLanguageModelService
{
    private readonly OpenAiOptions _openAiOptions;
    private readonly AnthropicOptions _anthropicOptions;
    private readonly Anthropic _anthropic;

    private readonly JsonSerializerOptions _jsonSerialisationOtions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public LargeLanguageModelService(IOptions<OpenAiOptions> openAiOptions, IOptions<AnthropicOptions> anthropicOptions) : base(openAiOptions.Value.ApiUrl)
    {
        HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiOptions.Value.ApiKey}");
        HttpClient.Timeout = TimeSpan.FromMinutes(3);
        _openAiOptions = openAiOptions.Value;
        _anthropicOptions = anthropicOptions.Value;
        _anthropic = new Anthropic
        {
            ApiKey = anthropicOptions.Value.ApiKey
        };
    }

    protected override string SerializeToJson<TBody>(TBody body)
    {
        // Serialize the object using snake_case
        return JsonSerializer.Serialize(body, _jsonSerialisationOtions);
    }

    public async Task<GptCreateEmbeddingResponse> CreateEmbeddings(string input)
    {
        var request = new GptCreateEmbeddingRequest
        {
            Model = _openAiOptions.EmbeddingModel,
            EncodingFormat = "float",
            Input = input
        };

        GptCreateEmbeddingResponse response = await PostAsync<GptCreateEmbeddingResponse, GptCreateEmbeddingRequest>("/embeddings", request, default);

        return response;
    }

    public async Task<IEnumerable<string>> ConvertToKeywordsAsync(string text, string? targetLanguage, CancellationToken cancellationToken)
    {
        var messageRequest = new MessageRequest()
        {
            Model = _anthropicOptions.LargeLanguageModelModel,
            MaxTokens = 1024,
            System = @$"
                You must extract 20 keywords from the following legal document that perfectly summarises the document, in JSON list format. The language of the keywords be in {targetLanguage ?? "English"}
                Here's an example of the format:
                [
                    ""Prospectus"",
                    ""Registration document"",
                    ""Securities note"",
                    ""Equity securities"",
                    ""Non-equity securities"",
                    ""Underlying asset"",
                    ""Guarantees"",
                    ""Scrutiny"",
                    ""Approval"",
                    ""Format"",
                    ""Content"",
                    ""EU Growth prospectus"",
                    ""Summary"",
                    ""Risk factors"",
                    ""Annexes"",
                    ""Issuers"",
                    ""Offerors"",
                    ""Admission to trading"",
                    ""Regulated market"",
                    ""Delegated regulation""
                ]
                ONLY RESPOND WITH THE JSON
                ",
            Messages = [new Message { Role = "user", Content = text }]
        };

        // Get the keywords using the LLM
        MessageResponse response = await _anthropic.Messages.CreateAsync(messageRequest, null, cancellationToken);

        string responseString = response.Content.FirstOrDefault()?.Text
            ?? throw new Exception("Anthropic returned 0 results");

        int startIndex = responseString.IndexOf('[');
        int endIndex = responseString.LastIndexOf(']');
        if (startIndex == -1 || endIndex == -1)
        {
            throw new FormatException("Invalid JSON format received from AI model.");
        }

        string jsonArray = responseString.Substring(startIndex, endIndex - startIndex + 1);
        var keywordsArray = JsonSerializer.Deserialize<IEnumerable<string>>(jsonArray);

        return keywordsArray ?? throw new InvalidOperationException("Deserialization resulted in null.");
    }

    public async Task<IEnumerable<float>> CreateEmbeddingsAsync(string text, CancellationToken cancellationToken)
    {
        var request = new GptCreateEmbeddingRequest
        {
            Model = _openAiOptions.EmbeddingModel,
            EncodingFormat = "float",
            Input = text
        };

        GptCreateEmbeddingResponse response = await PostAsync<GptCreateEmbeddingResponse, GptCreateEmbeddingRequest>("/embeddings", request, default);
        return response.Data.First().Embedding;
    }

    public async Task<(string, IEnumerable<string>)> SummariseDocumentAsync(string toSumarise, string targetLanguage, IEnumerable<EurLexSumDocument> similarDocuments, CancellationToken cancellationToken)
    {
        IEnumerable<string> jsonSimilarDocuments = similarDocuments.Select(x =>
        {
            return new
            {
                FullText = x.Reference,
                Summary = x.Summary,
                Keywords = x.Keywords,
            };
        })
            .Select(x => JsonSerializer.Serialize(x))
            .ToList();

        var messageRequest = new MessageRequest()
        {
            Model = _anthropicOptions.LargeLanguageModelModel,
            MaxTokens = 4096,
            System = @$"
                You must summarise any legal document given to you. The summary MUST maintain the legal integrity and truth of the original document. Make your summaries in {targetLanguage ?? "English"}.
                Here's an example of some similar summarised documents.
                {
                    // Map over the similarDocuments and show a Full: and Summarised: version of each, making sure the end prompt is clear
                    string.Join("\n\n", jsonSimilarDocuments)}

                YOU MUST REPLY IN THE FOLLOWING SCHEMA:
                {{
                  ""Summary"": """",
                  ""Keywords"": """",
                  ""SummarizedTitle"": """"
                }}
                ONLY REPLY WITH JSON. YOUR FULL REPLY MUST BE VALID JSON. DO NOT INCLUDE THE FULLTEXT IN YOUR RESPONSE.
                ",
            Messages = [new Message { Role = "user", Content = toSumarise }]
        };

        // Get the keywords using the LLM
        MessageResponse response = await _anthropic.Messages.CreateAsync(messageRequest, null, cancellationToken);

        string responseString = response.Content.FirstOrDefault()?.Text
            ?? throw new Exception("Anthropic returned 0 results");

        var deserialisedResponse = JsonSerializer.Deserialize<SummariseDocumentResponse>(responseString);

        return (deserialisedResponse.Summary, deserialisedResponse.Keywords);
    }
}

internal class SummariseDocumentResponse
{
    public string Summary { get; set; }

    public IEnumerable<string> Keywords { get; set; }
}
