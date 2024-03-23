namespace Atlas.Shared.Domain.Exceptions;

public class ErrorException : Exception
{
    public ErrorException(IEnumerable<Error> errors)
    {
        Errors = errors;
    }

    public ErrorException(Error? error)
    {
        Errors = error is not null ? [error] : new List<Error>() { Error.UnknownError };
    }

    public IEnumerable<Error>? Errors { get; set; }
}
