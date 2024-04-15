using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Law.Application.Services;
using Atlas.Law.Domain.Entities.EurLexSumDocumentEntity;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Law.Application.CQRS.LegalDocuments.Queue.ProcessLegalDocumentSummaryJob;
using Atlas.Shared.Application.Abstractions.Messaging.Queue;
using Atlas.Law.Domain.Errors;
using Atlas.Shared.Domain;
using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;
using Atlas.Shared.Application.ModuleBridge;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Queue;
using Atlas.Law.Application.CQRS.LegalDocuments.Queue.DecreaseCredits;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Queue.ProcessLegalDocumentSummary;

internal sealed class ProcessLegalDocumentSummaryQueuedCommandHandler(
    IUnitOfWork unitOfWork, 
    ILegalDocumentRepository legalDocumentRepository, 
    IEurLexSumDocumentRepository eurLexSumDocumentRepository,
    IVectorDatabaseService vectorDatabaseService, 
    ILargeLanguageModelService largeLanguageModelService,
    IModuleBridge moduleBridge,
    IQueueWriter queueWriter
    ) : IQueuedCommandHandler<ProcessLegalDocumentSummaryQueuedCommand>
{
    public async Task Handle(ProcessLegalDocumentSummaryQueuedCommand request, CancellationToken cancellationToken)
    {
        LegalDocument legalDocument = await legalDocumentRepository.GetByIdAsync(request.LegalDocumentId, false, cancellationToken)
            ?? throw new ErrorException(LawDomainErrors.Law.LegalDocumentNotFound);

        bool hasCredits = await moduleBridge.DoesUserHaveCredits(legalDocument.UserId, cancellationToken);
        if (!hasCredits)
        {
            return; // If there's no credits, just return
        }

        // There may be a summary created from previous failed summary attempt. Delete it
        if (legalDocument.Summary is not null)
        {
            await LegalDocument.RemoveSummaryAsync(legalDocument, legalDocumentRepository, cancellationToken);
        }

        var summary = await LegalDocument.CreateSummaryAsync(legalDocument, legalDocumentRepository, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken); // Commit here immediately so the user can query the LegalDocument whilst it's processing and see that it is indeed processing

        // Converts the supplied document text to a series of keywords
        IEnumerable<string> keywords = await largeLanguageModelService.ConvertToKeywordsAsync(legalDocument.FullText, legalDocument.Language, new Dictionary<string, string>() 
        {
            { "Name of document file", legalDocument.Name }
        },cancellationToken);
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
        SummariseDocumentResult result = await largeLanguageModelService.SummariseDocumentAsync(legalDocument.FullText, legalDocument.Language, similarDocuments, cancellationToken);

        LegalDocumentSummary.SetSummary(summary, result.SummarisedText, result.SummarisedTitle, concatenatedKeywords);

        await queueWriter.WriteAsync(new DecreaseCreditsQueuedCommand(legalDocument.UserId), cancellationToken);
    }
}