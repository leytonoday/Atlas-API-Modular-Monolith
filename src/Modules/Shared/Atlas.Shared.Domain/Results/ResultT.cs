using Atlas.Shared.Domain.Errors;

namespace Atlas.Shared.Domain.Results;

/// <summary>
/// Represents the result of an operation with additional data, which can be either successful or failed with errors.
/// </summary>
/// <typeparam name="TData">The type of additional data associated with the result.</typeparam>
public class Result<TData> : Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TData}"/> class.
    /// </summary>
    /// <param name="data">The additional data associated with the result.</param>
    /// <param name="isSuccess">Indicates whether the operation was successful.</param>
    /// <param name="errors">The errors associated with the operation, if any.</param>
    protected internal Result(TData? data, bool isSuccess, IEnumerable<Error>? errors)
        : base(isSuccess, errors) =>
        Data = data;

    /// <summary>
    /// Gets the additional data associated with the result.
    /// </summary>
    public TData? Data { get; init; }
}
