using Atlas.Shared.Application.Abstractions.Messaging.Queue;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Queue.ProcessLegalDocumentSummary;

public sealed record ProcessLegalDocumentSummaryQueuedCommand(Guid LegalDocumentId) : QueuedCommand;
