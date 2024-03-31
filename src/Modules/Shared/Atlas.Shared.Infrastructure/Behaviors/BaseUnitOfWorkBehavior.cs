using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Domain;
using MediatR;

namespace Atlas.Shared.Infrastructure.Behaviors;

public class BaseUnitOfWorkBehavior<TRequest, TResponse, TUnitOfWork>(TUnitOfWork unitOfWork) : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : notnull
    where TUnitOfWork : IBaseUnitOfWork
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // If the request is a query or if it's called a query (maybe the programmer has directly implemented IRequest rather than IQuery<TResult>), then continue
        if (IsQueryRequest(request) || HasQueryInName())
        {
            return await next();
        }

        // We can now presume that this is a command, and therefore there should be some side-effect which should be persisted to the database

        // Execute the next step in the pipeline
        var response = await next();

        // Save changes to the database
        await unitOfWork.CommitAsync(cancellationToken);

        return response;
    }

    /// <summary>
    /// Determines if the <paramref name="request"/> implements the <see cref="IQuery{TResult}"/> interface.
    /// </summary>
    /// <param name="request">The request to check.</param>
    /// <returns>True of the <see cref="IQuery{TResult}"/> interface is implemented, false otherwise.</returns>
    private bool IsQueryRequest(TRequest request)
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
    private bool HasQueryInName()
    {
        string? name = typeof(TRequest).FullName?.Split(".").Last();
        return name is not null && name.Contains("query", StringComparison.InvariantCultureIgnoreCase);
    }
}
