using Atlas.Shared.Infrastructure.CommandQueue;
using Atlas.Shared.Infrastructure.Integration.Inbox;
using Atlas.Shared.Infrastructure.Integration.Outbox;
using Atlas.Shared.Application.Queue;
using Atlas.Shared.Infrastructure.Queue;

namespace Atlas.Shared.Infrastructure;

public static class SharedInfrastructureConstants
{
    /// <summary>
    /// A nested class containing constants for table names used in the database that are common for all modules.
    /// </summary>
    public static class TableNames
    {
        /// <summary>
        /// The table name for storing <see cref="OutboxMessage"/> entities.
        /// </summary>
        public const string OutboxMessages = nameof(OutboxMessages);

        /// <summary>
        /// The table name for storing <see cref="InboxMessage"/> entities.
        /// </summary>
        public const string InboxMessages = nameof(InboxMessages);

        /// <summary>
        /// The table name for storing <see cref="QueueMessage"/> entities.
        /// </summary>
        public const string QueueMessages = nameof(QueueMessages);

        /// <summary>
        /// The table name for storing <see cref="QueueMessageHandlerAcknowledgement"/> entities.
        /// </summary>
        public const string QueueMessageHandlerAcknowledgements = nameof(QueueMessageHandlerAcknowledgements);

        /// <summary>
        /// The table name for storing <see cref="InboxMessageHandlerAcknowledgement"/> entities.
        /// </summary>
        public const string InboxMessageHandlerAcknowledgements = nameof(InboxMessageHandlerAcknowledgements);
    }
}
