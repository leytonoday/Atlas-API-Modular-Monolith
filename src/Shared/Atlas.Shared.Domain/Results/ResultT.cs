namespace Atlas.Shared.Domain.Results;

public class Result<TData> : Result
{
    protected internal Result(TData? data, bool isSuccess, IEnumerable<Error>? errors)
        : base(isSuccess, errors) =>
        Data = data;

    public TData? Data { get; init; }
}