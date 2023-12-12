namespace TaskManager.Core.Exceptions;

public class SprintNullException : Exception
{
    private const string _message = "The an instance of Sprint is null!";
    public SprintNullException()
        : base(_message) { }
}
