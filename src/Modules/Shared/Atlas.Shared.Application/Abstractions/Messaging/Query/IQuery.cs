using MediatR;

namespace Atlas.Shared.Application.Abstractions.Messaging.Query;

/// <summary>
/// Represents a query object that returns a result of type TResult. This should be used for requests that return data, and have no side-effects.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the query.</typeparam>
public interface IQuery<out TResult> : IRequest<TResult>;