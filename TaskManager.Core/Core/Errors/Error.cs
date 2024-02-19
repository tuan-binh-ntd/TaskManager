namespace TaskManager.Core.Core.Errors;

public sealed class Error(string code, string message)
{
    public string Code { get; } = code;

    public string Message { get; } = message;

    public static implicit operator string(Error error) => error?.Code ?? string.Empty;

    public IEnumerable<object> GetAtomicValues()
    {
        yield return Code;
        yield return Message;
    }

    internal static Error None => new(string.Empty, string.Empty);

    public static Error NotFound => new(DomainErrorConstants.NotFoundErrorCode, DomainErrorConstants.NotFoundErrorMessage);
}
