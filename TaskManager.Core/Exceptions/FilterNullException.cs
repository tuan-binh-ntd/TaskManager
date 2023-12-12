namespace TaskManager.Core.Exceptions;

public class FilterNullException : Exception
{
    private const string _message = "The an instance of Filter is null!";
    public FilterNullException()
        : base(_message) { }
}
