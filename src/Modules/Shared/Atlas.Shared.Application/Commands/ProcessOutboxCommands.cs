using Atlas.Shared.Application.Abstractions.Messaging;
using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Shared.Application.Commands;

/// <summary>
/// A command used to indicate that the contents of the module's Outbox should be processed.
/// </summary>
public class ProcessOutboxCommand : ICommand, IRecurringCommand;