using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Commands.DeleteLegalDocument;

public sealed record DeleteLegalDocumentCommand(Guid LegalDocumentId) : ICommand;
