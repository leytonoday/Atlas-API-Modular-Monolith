using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Commands.PrepareLegalDocumentSummary;

public sealed record PrepareLegalDocumentSummaryCommand(string DocumentText, string TargetLanguageName) : ICommand;
