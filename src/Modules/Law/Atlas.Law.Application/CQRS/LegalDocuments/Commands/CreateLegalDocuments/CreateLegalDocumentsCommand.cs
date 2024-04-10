using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Commands.CreateLegalDocuments;

public sealed record CreateLegalDocumentsCommand(IEnumerable<CreateDocument> Documents) : ICommand<IEnumerable<string>>;

public sealed record CreateDocument(string MimeType, string Base64Data, string FileName);