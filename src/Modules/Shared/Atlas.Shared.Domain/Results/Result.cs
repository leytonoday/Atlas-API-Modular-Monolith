using Atlas.Shared.Domain.Errors;

namespace Atlas.Shared.Domain.Results;

public class Result
{
    protected internal Result(bool isSuccess, IEnumerable<Error>? errors)
    {
        if (isSuccess && errors is not null && errors.Any())
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && (errors is null || !errors.Any()))
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; }

    public IEnumerable<Error>? Errors { get; }

    public static Result Success() => new(true, null);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, null);

    public static Result Failure(Error error) =>
        new(false, new List<Error>() { error });

    public static Result<TValue> Failure<TValue>(Error error) =>
        new(default, false, new List<Error>() { error });

    public static Result Failure(IEnumerable<Error>? errors) =>
        new(false, errors);

    public static Result<TValue> Failure<TValue>(IEnumerable<Error>? errors) =>
        new(default, false, errors);
}

