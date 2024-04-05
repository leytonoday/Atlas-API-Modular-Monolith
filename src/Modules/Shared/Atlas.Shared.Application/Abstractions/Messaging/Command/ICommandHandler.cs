using MediatR;

namespace Atlas.Shared.Application.Abstractions.Messaging.Command;

/// <summary>
/// Represents a handler for command objects of type TCommand.
/// </summary>
/// <typeparam name="TCommand">The type of command handled by this handler.</typeparam>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand;

/// <summary>
/// Represents a handler for command objects of type TCommand returning a result of type TResult.
/// </summary>
/// <typeparam name="TCommand">The type of command handled by this handler.</typeparam>
/// <typeparam name="TResult">The type of result returned by the command.</typeparam>
public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>;