using Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity;

namespace Atlas.Law.Infrastructure;

internal static class LawConstants
{
    internal static class Database
    {
        internal const string SchemaName = "Law";
    }

    /// <summary>
    /// Provides constants for table names used in the database for the Law sub-domain.
    /// </summary>
    internal static class TableNames
    {
        /// <summary>
        /// Represents the table name for <see cref="LegalDocument"/> entities in the database.
        /// </summary>
        internal const string LegalDocuments = nameof(LegalDocuments);

        /// <summary>
        /// Represents the table name for <see cref="EurLexSumDocument"/> entities in the database.
        /// </summary>
        internal const string EurLexSumDocuments = nameof(EurLexSumDocuments);

        /// <summary>
        /// Represents the table name for <see cref="LegalDocumentSummary"/> entities in the database.
        /// </summary>
        internal const string LegalDocumentSummaries = nameof(LegalDocumentSummaries);
    }
}

