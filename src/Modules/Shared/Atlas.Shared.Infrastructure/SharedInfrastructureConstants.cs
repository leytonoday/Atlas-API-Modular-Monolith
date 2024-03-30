using Atlas.Shared.Infrastructure.CommandQueue;
using Atlas.Shared.Infrastructure.Integration.Inbox;
using Atlas.Shared.Infrastructure.Integration.Outbox;

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
        /// Represents the table name for <see cref="InboxMessage"/> entities in the database.
        /// </summary>
        public const string InboxMessages = nameof(InboxMessages);

        /// <summary>
        /// Represents the table name for <see cref="CommandQueueMessage"/> entities in the database.
        /// </summary>
        public const string CommandQueueMessages = nameof(CommandQueueMessages);
    }
}
