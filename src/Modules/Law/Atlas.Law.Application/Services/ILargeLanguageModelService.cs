using Atlas.Law.Domain.Entities.EurLexSumDocumentEntity;

namespace Atlas.Law.Application.Services;

public interface ILargeLanguageModelService
{
    /// <summary>
    /// Converts some text to a series of keywords that represent the text.
    /// </summary>
    public Task<IEnumerable<string>> ConvertToKeywordsAsync(string text, string? targetLanguage, CancellationToken cancellationToken);

    public Task<IEnumerable<float>> CreateEmbeddingsAsync(string text, CancellationToken cancellationToken);

    public Task<(string, IEnumerable<string>)> SummariseDocumentAsync(string toSumarise, string targetLanguage, IEnumerable<EurLexSumDocument> similarDocuments, CancellationToken cancellationToken);
}
