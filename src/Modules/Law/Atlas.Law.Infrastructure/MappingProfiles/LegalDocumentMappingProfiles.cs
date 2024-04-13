using Atlas.Law.Application.CQRS.LegalDocuments.Queries.Shared;
using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;
using AutoMapper;

namespace Atlas.Law.Infrastructure.MappingProfiles;

/// <summary>
/// Represents a set of AutoMapper mapping profiles for the <see cref="LegalDocument"/> entity and it's DTOs.
/// </summary>
public class LegalDocumentMappingProfiles : Profile
{
    /// <summary>
    /// Initialises a new instance of the <see cref="LegalDocumentMappingProfiles"/> class.
    /// </summary>
    public LegalDocumentMappingProfiles()
    {
        CreateMap<LegalDocument, LegalDocumentDto>();

        CreateMap<LegalDocumentSummary, LegalDocumentSummaryDto>();
    }
}
