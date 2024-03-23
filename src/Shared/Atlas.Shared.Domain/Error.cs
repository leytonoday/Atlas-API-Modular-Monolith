namespace Atlas.Shared.Domain;

public class Error: IEquatable<Error>
{
    public static readonly Error UnknownError = new(nameof(UnknownError), "An unknown error has occured.");

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Message { get; } = null!;

    public string Code { get; } = null!;

    public bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Message == other.Message && Code == other.Code;
    }

    public override bool Equals(object? obj)
    {
        return obj is Error error && Equals(error);
    }

    public override int GetHashCode() => HashCode.Combine(Code, Message);
}

