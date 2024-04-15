using Atlas.Law.Application.Services;
using Atlas.Law.Domain.Entities.EurLexSumDocumentEntity;
using Atlas.Law.Infrastructure.Options;
using Atlas.Law.Infrastructure.Services.LargeLanguageModelService.Requests;
using Atlas.Law.Infrastructure.Services.LargeLanguageModelService.Response;
using Atlas.Law.Infrastructure.Services.LargeLanguageModelService.Shared;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Atlas.Law.Infrastructure.Services.LargeLanguageModelService;

internal class OpenAiLargeLanguageModelService : BaseApiService, ILargeLanguageModelService
{
    private readonly OpenAiOptions _openAiOptions;

    private readonly JsonSerializerOptions _jsonSerialisationOtions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public OpenAiLargeLanguageModelService(IOptions<OpenAiOptions> openAiOptions) : base(openAiOptions.Value.ApiUrl)
    {
        HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiOptions.Value.ApiKey}");
        HttpClient.Timeout = TimeSpan.FromMinutes(3);
        _openAiOptions = openAiOptions.Value;
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

    public async Task<IEnumerable<string>> ConvertToKeywordsAsync(string text, string? targetLanguage, IDictionary<string, string>? metadata, CancellationToken cancellationToken)
    {
        string prompt = @$"
You must extract 20 keywords from the following document that perfectly summarises, or that can be used to categorise the document, in JSON list format. The language of the keywords be in {targetLanguage}
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
DO NOT REPLY WITH AN EMPTY ARRAY. YOU MUST THINK OF SOME KEYWORDS.
Here is some data about the document:
{
    string.Join(",\n", metadata.Select(x => $"{x.Key}: {x.Value}").ToList())
}
ONLY RESPOND WITH THE JSON.
[
";

        GptCompletionResponse response = await ChatCompleteAsync(new()
        {
            new ()
            {
                Role = "system",
                Content = prompt
            },
            new ()
            {
                Role = "user",
                Content = text
            }
        },
        _openAiOptions.Gpt3Model
        );

        string responseString = response.Choices.First().Message.Content;

        int startOfArray = responseString.IndexOf('[');
        int endOfArray = responseString.LastIndexOf(']') + 1;

        string jsonArray = responseString.Substring(startOfArray, endOfArray - startOfArray);

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

    public async Task<SummariseDocumentResult> SummariseDocumentAsync(string toSumarise, string targetLanguage, IEnumerable<EurLexSumDocument> similarDocuments, CancellationToken cancellationToken)
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

        string prompt = @$"
                You must summarise any legal document given to you. The summary MUST maintain the legal integrity and truth of the original document. Make your summaries in {targetLanguage ?? "English"}.
                Here's an example of some similar summarised documents.
                {
                    // Map over the similarDocuments and show a Full: and Summarised: version of each, making sure the end prompt is clear
                    string.Join("\n\n", jsonSimilarDocuments)}

                YOU MUST REPLY IN THE FOLLOWING SCHEMA:
                {{
                  ""Summary"": """",
                  ""Keywords"": [],
                  ""SummarizedTitle"": """"
                }}
                ONLY REPLY WITH JSON. YOUR FULL REPLY MUST BE VALID JSON. DO NOT INCLUDE THE FULLTEXT IN YOUR RESPONSE. IT MUST BE VALID JSON OR BAD THINGS WILL HAPPEN!!";

        GptCompletionResponse response = await ChatCompleteAsync(new()
        {
            new ()
            {
                Role = "system",
                Content = prompt
            },
            new ()
            {
                Role = "user",
                Content = toSumarise
            }
        },
            _openAiOptions.Gpt4Model,
            true
        );

        string responseString = response.Choices.First().Message.Content;

        int startIndex = responseString.IndexOf('{');
        int endIndex = responseString.LastIndexOf('}');
        if (startIndex == -1 || endIndex == -1)
        {
            throw new FormatException("Invalid JSON format received from AI model.");
        }

        string jsonObject = responseString.Substring(startIndex, endIndex - startIndex + 1);

        var deserialisedResponse = JsonSerializer.Deserialize<SummariseDocumentResponse>(jsonObject);

        return new SummariseDocumentResult(deserialisedResponse?.Summary ?? "", deserialisedResponse?.SummarizedTitle ?? "", deserialisedResponse?.Keywords ?? Enumerable.Empty<string>());
    }

    private async Task<GptCompletionResponse> ChatCompleteAsync(List<GptCompletionMessage> messages, string model, bool jsonMode = false)
    {
        var request = new GptCompletionRequest
        {
            Model = model,
            Temperature = 0.1,
            Messages = messages,
            ResponseFormat = jsonMode ? new GptCompletionRequestResponseFormat
            {
                Type = "json_object"
            } : null
        };

        GptCompletionResponse response = await PostAsync<GptCompletionResponse, GptCompletionRequest>($"/chat/completions", request, default);

        return response;
    }
}

internal class SummariseDocumentResponse
{
    public string Summary { get; set; }

    public string SummarizedTitle { get; set; }

    public IEnumerable<string> Keywords { get; set; }
}
