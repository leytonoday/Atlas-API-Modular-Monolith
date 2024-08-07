﻿using Atlas.Plans.Domain.Entities.CreditTrackerEntity;

namespace Atlas.Plans.Infrastructure;

internal static class PlansConstants
{
    internal static class Database
    {
        internal const string SchemaName = "Plans";
    }

    /// <summary>
    /// Provides constants for table names used in the database for the Plans sub-domain.
    /// </summary>
    internal static class TableNames
    {
        /// <summary>
        /// Represents the table name for <see cref="Feature"/> entities in the database.
        /// </summary>
        internal const string Features = nameof(Features);

        /// <summary>
        /// Represents the table name for <see cref="CreditTracker"/> entities in the database.
        /// </summary>
        internal const string CreditTrackers = nameof(CreditTrackers);

        /// <summary>
        /// Represents the table name for <see cref="Plan"/> entities in the database.
        /// </summary>
        internal const string Plans = nameof(Plans);

        /// <summary>
        /// Represents the table name for <see cref="PlanFeature"/> entities in the database.
        /// </summary>
        internal const string PlanFeatures = nameof(PlanFeatures);

        /// <summary>
        /// Represents the table name for <see cref="StripeCustomer"/> entities in the database.
        /// </summary>
        internal const string StripeCustomers = nameof(StripeCustomers);

        /// <summary>
        /// Represents the table name for <see cref="StripeCardFingerprint"/> entities in the database.
        /// </summary>
        internal const string StripeCardFingerprints = nameof(StripeCardFingerprints);
    }
}
