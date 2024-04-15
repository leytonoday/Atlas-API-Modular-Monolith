using Atlas.Law.Domain.Entities.EurLexSumDocumentEntity;

namespace Atlas.Law.Application.Services;

/// <summary>
/// Service interface for interacting with a large language model.
/// </summary>
public interface ILargeLanguageModelService
{
    /// <summary>
    /// Converts the provided text into a series of keywords that represent the text.
    /// </summary>
    /// <param name="text">The text to convert into keywords.</param>
    /// <param name="targetLanguage">The target language for keyword extraction (optional).</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A task representing the asynchronous operation that yields a collection of keywords.</returns>
    public Task<IEnumerable<string>> ConvertToKeywordsAsync(string text, string? targetLanguage, CancellationToken cancellationToken);

    /// <summary>
    /// Creates embeddings for the provided text.
    /// </summary>
    /// <param name="text">The text to create embeddings for.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A task representing the asynchronous operation that yields a collection of embeddings.</returns>
    public Task<IEnumerable<float>> CreateEmbeddingsAsync(string text, CancellationToken cancellationToken);

    /// <summary>
    /// Summarizes the provided document.
    /// </summary>
    /// <param name="toSummarise">The document to summarize.</param>
    /// <param name="targetLanguage">The target language for the summary.</param>
    /// <param name="similarDocuments">A collection of similar documents to aid in summarization.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A task representing the asynchronous operation that yields a summarization result.</returns>
    public Task<SummariseDocumentResult> SummariseDocumentAsync(string toSumarise, string targetLanguage, IEnumerable<EurLexSumDocument> similarDocuments, CancellationToken cancellationToken);
}

/// <summary>
/// Represents the result of summarizing a document.
/// </summary>
public record SummariseDocumentResult (string SummarisedText, string SummarisedTitle, IEnumerable<string> Keywords);