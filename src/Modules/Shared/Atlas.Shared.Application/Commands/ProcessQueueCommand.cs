using Atlas.Shared.Application.Abstractions.Messaging;
using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Shared.Application.Commands;

public class ProcessQueueCommand : ICommand, IRecurringCommand;