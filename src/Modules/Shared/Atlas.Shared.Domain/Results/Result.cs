using Atlas.Shared.Domain.Errors;

namespace Atlas.Shared.Domain.Results;

/// <summary>
/// Represents the result of an operation, which can be either successful or failed with errors.
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the operation was successful.</param>
    /// <param name="errors">The errors associated with the operation, if any.</param>
    protected internal Result(bool isSuccess, IEnumerable<Error>? errors)
    {
        if (isSuccess && errors?.Any() == true)
        {
            throw new InvalidOperationException("Success result cannot have errors.");
        }

        if (!isSuccess && (errors is null || !errors.Any()))
        {
            throw new InvalidOperationException("Failure result must have at least one error.");
        }

        IsSuccess = isSuccess;
        Errors = errors;
    }

    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the errors associated with the operation, if any.
    /// </summary>
    public IEnumerable<Error>? Errors { get; }

    /// <summary>
    /// Creates a successful result with no errors.
    /// </summary>
    /// <returns>A successful result.</returns>
    public static Result Success() => new(true, null);

    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value associated with the successful result.</param>
    /// <returns>A successful result with the specified value.</returns>
    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, null);

    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error associated with the failed result.</param>
    /// <returns>A failed result with the specified error.</returns>
    public static Result Failure(Error error) =>
        new(false, new List<Error> { error });

    /// <summary>
    /// Creates a failed result with the specified errors.
    /// </summary>
    /// <param name="errors">The errors associated with the failed result.</param>
    /// <returns>A failed result with the specified errors.</returns>
    public static Result Failure(IEnumerable<Error>? errors) =>
        new(false, errors);
}