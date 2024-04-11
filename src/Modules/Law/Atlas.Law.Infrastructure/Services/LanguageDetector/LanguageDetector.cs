using Atlas.Law.Application.Services;
using Atlas.Law.Infrastructure.Options;
using Atlas.Law.Infrastructure.Services.LanguageDetector.Requests;
using Atlas.Law.Infrastructure.Services.LanguageDetector.Responses;
using Microsoft.Extensions.Options;

namespace Atlas.Law.Infrastructure.Services.LanguageDetector;

internal class GoogleTranslateLanguageDetector : BaseApiService, ILanguageDetector
{
    public GoogleTranslateLanguageDetector(IOptions<GoogleTranslateOptions> options) : base("https://translation.googleapis.com/language/translate/v2/detect")
    {
        HttpClient.DefaultRequestHeaders.Add("X-goog-api-key", options.Value.ApiKey);
    }

    public async Task<string?> DetectLanguageAsync(string text, CancellationToken cancellationToken)
    {
        var request = new DetectLanguageRequest(text);

        var response = await PostAsync<DetectLanguageResponse, DetectLanguageRequest>("", request, cancellationToken);

        return response.Data.Detections.FirstOrDefault()?.FirstOrDefault()?.Language ?? null;
    }
}

