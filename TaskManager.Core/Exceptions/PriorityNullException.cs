namespace TaskManager.Core.Exceptions;

public class PriorityNullException : Exception
{
    private const string _message = "The an instance of Priority is null!";
    public PriorityNullException()
        : base(_message) { }
}