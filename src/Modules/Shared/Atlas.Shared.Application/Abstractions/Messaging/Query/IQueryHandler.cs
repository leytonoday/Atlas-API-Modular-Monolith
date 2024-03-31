using MediatR;

namespace Atlas.Shared.Application.Abstractions.Messaging.Query;

/// <summary>
/// Represents a handler for query objects of type TQuery returning a result of type TResult.
/// </summary>
/// <typeparam name="TQuery">The type of query handled by this handler.</typeparam>
/// <typeparam name="TResult">The type of result returned by the query.</typeparam>
public interface IQueryHandler<in TQuery, TResult> :
    IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>;