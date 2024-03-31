namespace Atlas.Shared.Application.Abstractions.Messaging;

/// <summary>
/// A marker interface indicating that this command will recur and be handled very often.
/// Might indicate that this command shouldn't logged.
/// </summary>
public interface IRecurringCommand;