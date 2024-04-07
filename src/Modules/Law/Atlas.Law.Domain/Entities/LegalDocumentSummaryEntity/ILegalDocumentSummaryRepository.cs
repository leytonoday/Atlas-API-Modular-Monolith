using Atlas.Shared.Domain.Persistance;

namespace Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;

/// <summary>
/// Represents a repository for managing <see cref="LegalDocumentSummary"/> entities.
/// </summary>
public interface ILegalDocumentSummaryRepository : IRepository<LegalDocumentSummary, Guid>;