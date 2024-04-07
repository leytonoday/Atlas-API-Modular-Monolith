using Atlas.Shared.Domain.Persistance;

namespace Atlas.Law.Domain.Entities.LegalDocumentEntity;

/// <summary>
/// Represents a repository for managing <see cref="LegalDocument"/> entities.
/// </summary>
public interface ILegalDocumentRepository : IRepository<LegalDocument, Guid>;