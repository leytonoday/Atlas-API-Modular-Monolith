using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Commands.CreateLegalDocumentSummaryJob;

public sealed record CreateLegalDocumentSummaryJobCommand(string DocumentText, string TargetLanguageName) : ICommand;
