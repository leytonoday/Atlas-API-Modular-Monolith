using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Commands.CreateLegalDocumentSummaryJob;

internal sealed class CreateLegalDocumentSummaryJobCommandHandler(ILegalDocumentRepository legalDocumentRepository, IExecutionContextAccessor executionContext) : ICommandHandler<CreateLegalDocumentSummaryJobCommand>
{
    public async Task Handle(CreateLegalDocumentSummaryJobCommand request, CancellationToken cancellationToken)
    {
        LegalDocument legalDocument = LegalDocument.Create(request.DocumentText, request.TargetLanguageName, executionContext.UserId);

        await legalDocumentRepository.AddAsync(legalDocument, cancellationToken);
    }
}
