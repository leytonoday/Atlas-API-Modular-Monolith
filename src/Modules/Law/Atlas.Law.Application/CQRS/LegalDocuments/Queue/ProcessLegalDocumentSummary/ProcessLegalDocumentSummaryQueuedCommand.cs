using Atlas.Shared.Application.Abstractions.Messaging.Queue;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Queue.ProcessLegalDocumentSummaryJob;

public sealed record ProcessLegalDocumentSummaryQueuedCommand(Guid LegalDocumentId) : QueuedCommand;
