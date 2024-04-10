using Atlas.Law.Application.CQRS.LegalDocuments.Queue.ProcessLegalDocumentSummaryJob;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Queue;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Commands.CreateLegalDocumentSummary;

internal sealed class CreateLegalDocumentSummaryCommandHandler(IQueueWriter queueWriter) : ICommandHandler<CreateLegalDocumentSummaryCommand>
{
    public async Task Handle(CreateLegalDocumentSummaryCommand request, CancellationToken cancellationToken)
    {
        await queueWriter.WriteAsync(new ProcessLegalDocumentSummaryQueuedCommand(request.LegalDocumentId), cancellationToken);
    }
}
