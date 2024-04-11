using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;
using Atlas.Law.Domain.Errors;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Domain.Exceptions;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Commands.DeleteLegalDocument;

internal sealed class DeleteLegalDocumentCommandHandler(
    ILegalDocumentRepository legalDocumentRepository,
    ILegalDocumentSummaryRepository legalDocumentSummaryRepository,
    IExecutionContextAccessor executionContextAccessor) : ICommandHandler<DeleteLegalDocumentCommand>
{
    public async Task Handle(DeleteLegalDocumentCommand request, CancellationToken cancellationToken)
    {
        LegalDocument toDelete = await legalDocumentRepository.GetByIdAsync(request.LegalDocumentId, true, cancellationToken)
            ?? throw new ErrorException(LawDomainErrors.Law.LegalDocumentNotFound);

        if (toDelete.UserId != executionContextAccessor.UserId)
        {
            throw new ErrorException(LawDomainErrors.Law.CanOnlyDeleteOwnLegalDocuments);
        }

        await LegalDocument.DeleteAsync(toDelete, legalDocumentRepository, legalDocumentSummaryRepository, cancellationToken);
    }
}
