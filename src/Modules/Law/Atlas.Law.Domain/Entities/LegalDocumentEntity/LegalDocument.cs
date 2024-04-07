using Atlas.Law.Domain.Enums;
using Atlas.Shared.Domain.AggregateRoot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Law.Domain.Entities.LegalDocumentEntity;

public sealed class LegalDocument : AggregateRoot<Guid>
{
    public string FullText { get; private set; } = null!;

    public string Language { get; private set; } = null!;

    public string? SummarisedText { get; private set; }

    public string? SummarizedTitle { get; private set; }

    public string? Keywords { get; private set; }

    public LegalDocumentProcessingStatus ProcessingStatus { get; private set; } = LegalDocumentProcessingStatus.NOT_STARTED;

    public Guid UserId { get; private set; }

    public static LegalDocument Create(string fullText, string language, Guid userId)
    {
        Guid id = Guid.NewGuid();

        var legalDocument = new LegalDocument
        {
            Id = id,
            FullText = fullText,
            UserId = userId,
            Language = language
        };

        return legalDocument;
    }

    public static void SetSummary(LegalDocument legalDocument, string summarisedText, string summarisedKeywords)
    {
        legalDocument.SummarisedText = summarisedText;
        legalDocument.Keywords = summarisedKeywords;
        legalDocument.ProcessingStatus = LegalDocumentProcessingStatus.COMPLETE;
    }

    public static void SetAsProcessing(LegalDocument legalDocument)
    {
        legalDocument.ProcessingStatus = LegalDocumentProcessingStatus.PROCESSING;
    }
}