using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;
using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Law.Domain.Entities.LegalDocumentEntity.BusinessRules;

internal class LegalDocumentCannotBeDeletedWhilstSummaryIncompleteBusinessRule(LegalDocument legalDocument) : IBusinessRule
{
    public string Message => "A LegalDocument cannot be deleted if it has a summary that is not complete.";

    public string Code => $"LegalDocument.{nameof(LegalDocumentCannotBeDeletedWhilstSummaryIncompleteBusinessRule)}";


    public bool IsBroken()
    {
        if (legalDocument.Summary is not null && legalDocument.Summary.ProcessingStatus != Enums.LegalDocumentProcessingStatus.COMPLETE)
        {
            return true;
        }

        return false;
    }
}
