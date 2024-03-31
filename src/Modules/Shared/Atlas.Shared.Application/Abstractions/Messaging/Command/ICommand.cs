using MediatR;

namespace Atlas.Shared.Application.Abstractions.Messaging.Command;

/// <summary>
/// Represents a command object that returns a result of type TResult. This should be used when the request has some side-effect and returns data.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the command.</typeparam>
public interface ICommand<out TResult> : IRequest<TResult>;

/// <summary>
/// Represents a command object without a specific result. This should be used when the request has some side-effect and doesn't return data.
/// </summary>
public interface ICommand : IRequest;