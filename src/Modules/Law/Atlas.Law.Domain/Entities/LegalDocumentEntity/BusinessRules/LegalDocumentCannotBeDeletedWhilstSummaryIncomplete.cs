using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;
using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Law.Domain.Entities.LegalDocumentEntity.BusinessRules;

internal class LegalDocumentCannotBeDeletedWhilstSummaryIncomplete(Guid legalDocumentId, ILegalDocumentSummaryRepository legalDocumentSummaryRepository) : IAsyncBusinessRule
{
    public string Message => "A LegalDocument cannot be deleted if it has a summary that is not complete.";

    public string Code => $"LegalDocument.{nameof(LegalDocumentCannotBeDeletedWhilstSummaryIncomplete)}";


    public async Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
    {
        LegalDocumentSummary? summary = (await legalDocumentSummaryRepository.GetByConditionAsync(x => x.LegalDocumentId == legalDocumentId, false, cancellationToken)).FirstOrDefault();

        if (summary is not null && summary.ProcessingStatus != Enums.LegalDocumentProcessingStatus.COMPLETE)
        {
            return true;
        }

        return false;
    }
}
