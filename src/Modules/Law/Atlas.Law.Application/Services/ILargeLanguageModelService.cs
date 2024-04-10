using Atlas.Law.Domain.Entities.EurLexSumDocumentEntity;

namespace Atlas.Law.Application.Services;

public interface ILargeLanguageModelService
{
    /// <summary>
    /// Converts some text to a series of keywords that represent the text.
    /// </summary>
    public Task<IEnumerable<string>> ConvertToKeywordsAsync(string text, string? targetLanguage, CancellationToken cancellationToken);

    public Task<IEnumerable<float>> CreateEmbeddingsAsync(string text, CancellationToken cancellationToken);

    public Task<SummariseDocumentResult> SummariseDocumentAsync(string toSumarise, string targetLanguage, IEnumerable<EurLexSumDocument> similarDocuments, CancellationToken cancellationToken);
}

public record SummariseDocumentResult (string SummarisedText, string SummarisedTitle, IEnumerable<string> Keywords);