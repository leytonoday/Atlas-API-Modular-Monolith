using DocumentFormat.OpenXml.Drawing.Charts;
using System.Text.Json.Serialization;

namespace Atlas.Law.Infrastructure.Services.LanguageDetector.Responses;

internal class DetectLanguageResponseDetection
{
    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }
    
    [JsonPropertyName("language")]
    public string Language { get; set; }
}

internal class DetectLanguageResponseData
{
    [JsonPropertyName("detections")]
    public List<List<DetectLanguageResponseDetection>> Detections { get; set; }
}

internal class DetectLanguageResponse
{
    [JsonPropertyName("data")]
    public DetectLanguageResponseData Data { get; set; }
}
