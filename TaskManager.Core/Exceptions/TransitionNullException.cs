namespace TaskManager.Core.Exceptions;

public class TransitionNullException : Exception
{
    private const string _message = "The an instance of Transition is null!";
    public TransitionNullException()
        : base(_message) { }
}
