using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Shared.Application.Queue;
using Atlas.Law.Application.Services;
using Atlas.Law.Domain.Entities.EurLexSumDocumentEntity;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Law.Application.CQRS.LegalDocuments.Queue.ProcessLegalDocumentSummaryJob;
using Atlas.Shared.Application.Abstractions.Messaging.Queue;
using Atlas.Law.Domain.Errors;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Queue.ProcessLegalDocumentSummary;

internal sealed class ProcessLegalDocumentSummaryQueuedCommandHandler(ILegalDocumentRepository legalDocumentRepository, IEurLexSumDocumentRepository eurLexSumDocumentRepository, IVectorDatabaseService vectorDatabaseService, ILargeLanguageModelService largeLanguageModelService, IQueueWriter queueWriter) : IQueuedCommandHandler<ProcessLegalDocumentSummaryQueuedCommand>
{
    public async Task Handle(ProcessLegalDocumentSummaryQueuedCommand request, CancellationToken cancellationToken)
    {
        LegalDocument legalDocument = await legalDocumentRepository.GetByIdAsync(request.LegalDocumentId, true, cancellationToken)
            ?? throw new ErrorException(LawDomainErrors.Law.LegalDocumentNotFound);

        // Mark it as processing, so that if the generation of the summary takes a long time, and the user queries for the status of the job, they can see it's in the works.
        LegalDocument.SetAsProcessing(legalDocument);

        // Converts the supplied document text to a series of keywords
        IEnumerable<string> keywords = await largeLanguageModelService.ConvertToKeywordsAsync(legalDocument.FullText, legalDocument.Language, cancellationToken);
        string concatenatedKeywords = string.Join(", ", keywords);

        // Convert those keywords to embeddings (a list of floats, also known as a vector)
        IEnumerable<float> embedding = await largeLanguageModelService.CreateEmbeddingsAsync(concatenatedKeywords, cancellationToken);

        // Search the vector database for embeddings that are similar to this one
        // The Ids of those embeddings within the vector database is made up of the CelexId and document language
        IEnumerable<string> similarDocumentIds = await vectorDatabaseService.GetSimilarVectorIdsAsync(embedding, cancellationToken);

        var similarDocuments = new List<EurLexSumDocument>();

        // Extract the CelexId and Language from the Id of the vector embedding Id
        var documentInfos = similarDocumentIds.Select(id =>
        {
            string[] parts = id.Split("_");
            return new { CelexId = parts[0], Language = parts[1] };
        });

        // Fetching documents
        foreach (var documentInfo in documentInfos)
        {
            EurLexSumDocument document = (await eurLexSumDocumentRepository.GetByCelexIdAndLanguage(documentInfo.CelexId, documentInfo.Language, false, cancellationToken))!;
            similarDocuments.Add(document);
        }

        // Summarise the document into the provided language, using the similar documents as a reference
        (string summary, IEnumerable<string> summaryKeywords) = await largeLanguageModelService.SummariseDocumentAsync(legalDocument.FullText, legalDocument.Language, similarDocuments, cancellationToken);

        LegalDocument.SetSummary(legalDocument, summary, string.Join(',', keywords));
    }
}