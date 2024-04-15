using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Law.Domain.Entities.LegalDocumentEntity.BusinessRules;

/// <summary>
/// Represents a business rule stating that a legal document cannot be deleted if it has an incomplete summary.
/// </summary>
internal class LegalDocumentCannotBeDeletedWhilstSummaryIncompleteBusinessRule(LegalDocument legalDocument) : IBusinessRule
{
    /// <inheritdoc/>
    public string Message => "A LegalDocument cannot be deleted if it has a summary that is not complete.";

    /// <inheritdoc/>
    public string Code => $"LegalDocument.{nameof(LegalDocumentCannotBeDeletedWhilstSummaryIncompleteBusinessRule)}";

    /// <inheritdoc/>
    public bool IsBroken()
    {
        if (legalDocument.Summary is not null && legalDocument.Summary.ProcessingStatus != Enums.LegalDocumentProcessingStatus.COMPLETE)
        {
            return true;
        }

        return false;
    }
}
