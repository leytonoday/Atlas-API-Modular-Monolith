using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Atlas.Shared.Application.Behaviors;

/// <summary>
/// Represents a logging behavior for logging request and response data.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class LoggingBehaviour<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Handles the logging behavior for the specified request.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="next">The delegate representing the next step in the pipeline.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>The response of type <typeparamref name="TResponse"/>.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var name = typeof(TRequest).FullName!.Split(".").Last();
        var body = JsonSerializer.Serialize(request);

        if (name.StartsWith("Process"))
        {
            logger.LogDebug("Executing: {Name} - {Body}", name, body);
        }
        else
        {
            logger.LogInformation("Executing: {Name} - {Body}", name, body);
        }

        TResponse result;
        try
        {
            result = await next();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error Executing: {Name} - {Body}", name, body);
            throw;
        }

        return result;
    }
}