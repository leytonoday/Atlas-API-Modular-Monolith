namespace Atlas.Law.Application.Services;

/// <summary>
/// Represents a service for detecting the language of text.
/// </summary>
public interface ILanguageDetector
{
    /// <summary>
    /// Detects the language of the specified text asynchronously.
    /// </summary>
    /// <param name="text">The text for which to detect the language.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the detected language, or null if the language cannot be determined.</returns>
    public Task<string?> DetectLanguageAsync(string text, CancellationToken cancellationToken);
}
