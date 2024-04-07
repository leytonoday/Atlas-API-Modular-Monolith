using Atlas.Law.Application.CQRS.LegalDocuments.Queue.ProcessLegalDocumentSummaryJob;
using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Queue;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Commands.PrepareLegalDocumentSummary;

internal sealed class PrepareLegalDocumentSummaryCommandHandler(ILegalDocumentRepository legalDocumentRepository, IExecutionContextAccessor executionContext, IQueueWriter queueWriter) : ICommandHandler<PrepareLegalDocumentSummaryCommand>
{
    public async Task Handle(PrepareLegalDocumentSummaryCommand request, CancellationToken cancellationToken)
    {
        LegalDocument legalDocument = LegalDocument.Create(request.DocumentText, request.TargetLanguageName, executionContext.UserId);

        await legalDocumentRepository.AddAsync(legalDocument, cancellationToken);

        await queueWriter.WriteAsync(new ProcessLegalDocumentSummaryQueuedCommand(legalDocument.Id), cancellationToken);
    }
}
