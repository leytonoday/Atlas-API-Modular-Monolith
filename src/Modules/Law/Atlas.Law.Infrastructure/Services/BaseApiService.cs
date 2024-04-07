using System.Text;
using System.Text.Json;

namespace Atlas.Law.Infrastructure.Services;


/// <summary>
/// Base class for API services providing common functionality for making HTTP requests.
/// </summary>
internal abstract class BaseApiService
{
    /// <summary>
    /// The HTTP client used for making requests.
    /// </summary>
    protected HttpClient HttpClient { get; private init; }

    /// <summary>
    /// The base URL for the API.
    /// </summary>
    protected string? BaseUrl { get; private init; }

    /// <summary>
    /// Constructs a new instance of the BaseApiService with the specified base URL.
    /// </summary>
    /// <param name="baseUrl">The base URL for the API.</param>
    protected BaseApiService(string? baseUrl)
    {
        BaseUrl = baseUrl;
        HttpClient = new HttpClient();
    }

    /// <summary>
    /// Sends an HTTP GET request asynchronously.
    /// </summary>
    /// <typeparam name="TResponse">The type of the expected response.</typeparam>
    /// <param name="path">The path for the request relative to the base URL.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <param name="newBaseUrl">An optional new base URL for the request.</param>
    /// <returns>The deserialized response of type TResponse.</returns>
    protected virtual async Task<TResponse> GetAsync<TResponse>(string path, CancellationToken cancellationToken, string? newBaseUrl = null)
    {
        string fullPath = $"{newBaseUrl ?? BaseUrl}{path}";

        HttpResponseMessage result = await HttpClient.GetAsync(fullPath, cancellationToken);

        string responseBody = await result.Content.ReadAsStringAsync(cancellationToken)
            ?? throw new Exception($"Could not fetch data from {fullPath}");

        TResponse? deserialized = JsonSerializer.Deserialize<TResponse>(responseBody)
            ?? throw new Exception($"Could not deserialize data received from {fullPath}");

        return deserialized;
    }

    /// <summary>
    /// Sends an HTTP POST request asynchronously.
    /// </summary>
    /// <typeparam name="TResponse">The type of the expected response.</typeparam>
    /// <typeparam name="TContent">The type of the request body.</typeparam>
    /// <param name="path">The path for the request relative to the base URL.</param>
    /// <param name="body">The content of the request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <param name="newBaseUrl">An optional new base URL for the request.</param>
    /// <returns>The deserialized response of type TResponse.</returns>
    protected virtual async Task<TResponse> PostAsync<TResponse, TContent>(string path, TContent body, CancellationToken cancellationToken, string? newBaseUrl = null)
    {
        string fullPath = $"{newBaseUrl ?? BaseUrl}{path}";

        var jsonBody = new StringContent(SerializeToJson(body), Encoding.UTF8, "application/json");

        var httpRequestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri(fullPath),
            Method = HttpMethod.Post,
            Content = jsonBody,
        };

        HttpResponseMessage result = await HttpClient.SendAsync(httpRequestMessage, cancellationToken);

        string responseBody = await result.Content.ReadAsStringAsync(cancellationToken)
            ?? throw new Exception($"Could not fetch data from {fullPath}");

        TResponse? deserialized = DeserialzeFromJson<TResponse>(responseBody)
            ?? throw new Exception($"Could not deserialize data received from {fullPath}");

        return deserialized;
    }

    /// <summary>
    /// Sends an HTTP POST request asynchronously without expecting a response.
    /// </summary>
    /// <typeparam name="TContent">The type of the request body.</typeparam>
    /// <param name="path">The path for the request relative to the base URL.</param>
    /// <param name="body">The content of the request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <param name="newBaseUrl">An optional new base URL for the request.</param>
    protected virtual async Task PostAsync<TContent>(string path, TContent body, CancellationToken cancellationToken, string? newBaseUrl = null)
    {
        string fullPath = $"{newBaseUrl ?? BaseUrl}{path}";

        var jsonBody = new StringContent(SerializeToJson(body), Encoding.UTF8, "application/json");

        var httpRequestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri(fullPath),
            Method = HttpMethod.Post,
            Content = jsonBody,
        };

        var result = await HttpClient.SendAsync(httpRequestMessage, cancellationToken);
        string responseBody = await result.Content.ReadAsStringAsync(cancellationToken)
            ?? throw new Exception($"Could not fetch data from {fullPath}");

        Console.WriteLine(result.StatusCode);

        return;
    }

    /// <summary>
    /// Serializes the object into a JSON string.
    /// </summary>
    /// <typeparam name="TBody">The type of the object to be serialized.</typeparam>
    /// <param name="body">The object to be serialized.</param>
    /// <returns>A JSON string representing the serialized object.</returns>
    protected virtual string SerializeToJson<TBody>(TBody body)
    {
        return JsonSerializer.Serialize(body);
    }

    /// <summary>
    /// Deserializes the JSON string into an object of the specified type.
    /// </summary>
    /// <typeparam name="TResponse">The type of the object to be deserialized.</typeparam>
    /// <param name="body">The JSON string to be deserialized.</param>
    /// <returns>An object of type TResponse deserialized from the JSON string.</returns>
    protected virtual TResponse? DeserialzeFromJson<TResponse>(string body)
    {
        return JsonSerializer.Deserialize<TResponse>(body);
    }
}
