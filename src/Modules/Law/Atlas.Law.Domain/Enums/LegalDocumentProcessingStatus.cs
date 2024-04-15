namespace Atlas.Law.Domain.Enums;

/// <summary>
/// Indicates the status some operation that is being performed on a <see cref="LegalDocument"/>
/// </summary>
public enum LegalDocumentProcessingStatus
{
    NOT_STARTED, 
    PROCESSING,
    COMPLETE,
}