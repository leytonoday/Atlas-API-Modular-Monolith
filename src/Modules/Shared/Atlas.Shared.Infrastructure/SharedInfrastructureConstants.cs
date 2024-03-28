namespace Atlas.Shared.Infrastructure;

public static class SharedInfrastructureConstants
{
    public static class TableNames
    {
        /// <summary>
        /// Represents the table name for <see cref="OutboxMessage"/> entities in the database.
        /// </summary>
        public const string OutboxMessages = nameof(OutboxMessages);

        /// <summary>
        /// Represents the table name for <see cref="OutboxMessageConsumerAcknowledgement"/> entities in the database.
        /// </summary>
        public const string OutboxMessageConsumerAcknowledgements = nameof(OutboxMessageConsumerAcknowledgements);
    }
}
