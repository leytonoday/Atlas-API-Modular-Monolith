using Atlas.Law.Application.Services;
using Atlas.Law.Infrastructure.Options;
using Atlas.Law.Infrastructure.Services.LanguageDetector.Requests;
using Atlas.Law.Infrastructure.Services.LanguageDetector.Responses;
using Microsoft.Extensions.Options;

namespace Atlas.Law.Infrastructure.Services.LanguageDetector;

/// <summary>
/// Provides language detection using the Google Translate API.
/// </summary>
internal class GoogleTranslateLanguageDetector : BaseApiService, ILanguageDetector
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleTranslateLanguageDetector"/> class.
    /// </summary>
    /// <param name="options">The options for configuring the Google Translate API.</param>
    public GoogleTranslateLanguageDetector(IOptions<GoogleTranslateOptions> options) : base("https://translation.googleapis.com/language/translate/v2/detect")
    {
        HttpClient.DefaultRequestHeaders.Add("X-goog-api-key", options.Value.ApiKey);
    }

    /// <inheritdoc/>
    public async Task<string?> DetectLanguageAsync(string text, CancellationToken cancellationToken)
    {
        var request = new DetectLanguageRequest(text);

        var response = await PostAsync<DetectLanguageResponse, DetectLanguageRequest>("", request, cancellationToken);

        return response.Data.Detections.FirstOrDefault()?.FirstOrDefault()?.Language ?? null;
    }
}

