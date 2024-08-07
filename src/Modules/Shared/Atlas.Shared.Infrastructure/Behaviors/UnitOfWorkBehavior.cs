﻿using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Domain;
using MediatR;

namespace Atlas.Shared.Infrastructure.Behaviors;

/// <summary>
/// Implements a pipeline behavior that automatically manages UnitOfWork for commands within a MediatR pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of request being processed.</typeparam>
/// <typeparam name="TResponse">The type of response returned by the request handler.</typeparam>
public class UnitOfWorkBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork) : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // If the request is a query or if it's called a query (sanity check. Maybe the programmer has directly implemented IRequest rather than IQuery<TResult>), then continue
        if (UnitOfWorkBehavior<TRequest, TResponse>.IsQueryRequest(request) || UnitOfWorkBehavior<TRequest, TResponse>.HasQueryInName())
        {
            return await next();
        }

        // We can now presume that this is a command rather than a query, and therefore there should be some side-effect which
        // should be persisted to the database. Therefore, start a transaction. EF Core change tracker will handle track everything, 
        // so we don't need to wrap the "next" in the transaction. 

        // Execute the next step in the pipeline
        var response = await next();

        // Save changes to the database if there have been any
        if (unitOfWork.HasUnsavedChanges())
        {
            await unitOfWork.CommitAsync(cancellationToken);
        }

        return response;
    }

    /// <summary>
    /// Determines if the <paramref name="request"/> implements the <see cref="IQuery{TResult}"/> interface.
    /// </summary>
    /// <param name="request">The request to check.</param>
    /// <returns>True of the <see cref="IQuery{TResult}"/> interface is implemented, false otherwise.</returns>
    private static bool IsQueryRequest(TRequest request)
    {
        Type requestType = request.GetType();

        Type iQueryType = typeof(IQuery<>);

        // Iterate over each interface that this request type implements. If it's an IQuery, then return true.
        foreach (Type iface in requestType.GetInterfaces())
        {
            if (iface.IsGenericType && iface.GetGenericTypeDefinition() == iQueryType)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Determines if the <typeparamref name="TRequest"/> type has the substring "query" in it.
    /// </summary>
    /// <remarks>
    /// Ideally, the <typeparamref name="TRequest"/> should be of type <see cref="IQuery{TResult}"/> if it's supposed to be a query, but who knows maybe the programmer forgot and has
    /// erroneously directly implemented <see cref="IRequest{TResult}"/> on the query. Therefore, we also check the name of the request as a sanity check.
    /// </remarks>
    /// <returns>True if the type name has "query" in it, false otherwise.</returns>
    private static bool HasQueryInName()
    {
        string? name = typeof(TRequest).FullName?.Split(".").Last();
        return name is not null && name.Contains("query", StringComparison.InvariantCultureIgnoreCase);
    }
}
