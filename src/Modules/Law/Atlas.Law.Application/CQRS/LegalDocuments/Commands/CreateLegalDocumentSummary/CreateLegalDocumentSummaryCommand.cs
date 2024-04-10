using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Commands.CreateLegalDocumentSummary;

public record CreateLegalDocumentSummaryCommand(Guid LegalDocumentId) : ICommand;
