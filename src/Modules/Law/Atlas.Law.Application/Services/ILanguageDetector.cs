namespace Atlas.Law.Application.Services;

public interface ILanguageDetector
{
    public Task<string?> DetectLanguageAsync(string text, CancellationToken cancellationToken);
}
