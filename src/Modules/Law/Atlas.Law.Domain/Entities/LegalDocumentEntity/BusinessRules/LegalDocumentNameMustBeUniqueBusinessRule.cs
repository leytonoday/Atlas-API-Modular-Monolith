using Atlas.Shared.Domain.BusinessRules;

namespace Atlas.Law.Domain.Entities.LegalDocumentEntity.BusinessRules;

/// <summary>
/// A user cannot have two <see cref="LegalDocument"/>s of the same name.
/// </summary>
/// <param name="name">The name of the legal document.</param>
/// <param name="userId">The Id of the user that the legal documents belong to.</param>
/// <param name="legalDocumentRepository">The repository to check for hte existance of other legal documents.</param>
internal sealed class LegalDocumentNameMustBeUniqueBusinessRule(string name, Guid userId, ILegalDocumentRepository legalDocumentRepository) : IAsyncBusinessRule
{
    public string Message => "Legal documents must have have unique names.";

    public string Code => $"LegalDocument.{nameof(LegalDocumentNameMustBeUniqueBusinessRule)}";

    public async Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
    {
        LegalDocument? existing = await legalDocumentRepository.GetByNameAndUserAsync(name, userId, false, cancellationToken);
        return existing is not null;
    }
}
