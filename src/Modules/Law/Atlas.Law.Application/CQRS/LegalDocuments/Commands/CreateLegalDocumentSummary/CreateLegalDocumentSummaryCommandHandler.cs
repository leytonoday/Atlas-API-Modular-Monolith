using Atlas.Law.Application.CQRS.LegalDocuments.Queue.ProcessLegalDocumentSummary;
using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Law.Domain.Errors;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.ModuleBridge;
using Atlas.Shared.Application.Queue;
using Atlas.Shared.Domain.Exceptions;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Commands.CreateLegalDocumentSummary;

internal sealed class CreateLegalDocumentSummaryCommandHandler(
    IQueueWriter queueWriter, 
    IModuleBridge moduleBridge,
    ILegalDocumentRepository legalDocumentRepository) : ICommandHandler<CreateLegalDocumentSummaryCommand>
{
    public async Task Handle(CreateLegalDocumentSummaryCommand request, CancellationToken cancellationToken)
    {
        LegalDocument legalDocument = await legalDocumentRepository.GetByIdAsync(request.LegalDocumentId, false, cancellationToken)
                                      ?? throw new ErrorException(LawDomainErrors.Law.LegalDocumentNotFound);

        bool hasCredits = await moduleBridge.DoesUserHaveCredits(legalDocument.UserId, cancellationToken);
        if (!hasCredits)
        {
            throw new ErrorException(LawDomainErrors.Law.NotEnoughCredits);
        }

        // There may be a summary created from previous failed summary attempt. Delete it. Not really sure if this is necessary, mostly a sanity check
        if (legalDocument.Summary is not null)
        {
            await LegalDocument.RemoveSummaryAsync(legalDocument, legalDocumentRepository, cancellationToken);
        }

        await LegalDocument.CreateSummaryAsync(legalDocument, legalDocumentRepository, cancellationToken);
        
        await queueWriter.WriteAsync(new ProcessLegalDocumentSummaryQueuedCommand(request.LegalDocumentId), cancellationToken);
    }
}
