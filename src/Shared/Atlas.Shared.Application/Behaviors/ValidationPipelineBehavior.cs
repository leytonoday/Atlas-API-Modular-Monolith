using Atlas.Shared.Domain.Errors;
using Atlas.Shared.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Atlas.Shared.Application.Behaviors;

/// <summary>
/// Pipeline behavior responsible for validating the specified <typeparamref name="TRequest"/> using registered FluentValidation validators
/// and returning a <typeparamref name="TResponse"/> with validation errors if any.
/// </summary>
/// <typeparam name="TRequest">The type of the request to be validated.</typeparam>
/// <typeparam name="TResponse">The type of the response, expected to be derived from <see cref="Result"/>.</typeparam>
/// <param name="validators">The collection of validators to be applied to the request.</param>
public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the validation of the request and executes the next step in the pipeline if validation succeeds.
    /// </summary>
    /// <param name="request">The request to be validated.</param>
    /// <param name="next">The next step in the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token for handling asynchronous operations.</param>
    /// <returns>A <typeparamref name="TResponse"/> with validation errors if any, or the result of the next step in the pipeline.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Validate the request and collect errors
        var validationErrors = new List<Error>();
        foreach (var validator in validators)
        {
            var results = await validator.ValidateAsync(request, cancellationToken);
            if (results.Errors.Any())
            {
                validationErrors.AddRange(results.Errors.Select(f => new Error(f.PropertyName, f.ErrorMessage)));
            }
        }

        // If there are any errors, throw error
        if (validationErrors.Any())
        {
            throw new ErrorException(validationErrors);
        }

        // Otherwise, continue with the pipeline
        return await next();
    }
}