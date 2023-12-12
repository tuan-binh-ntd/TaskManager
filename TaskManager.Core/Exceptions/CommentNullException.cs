namespace TaskManager.Core.Exceptions;

public class CommentNullException : Exception
{
    private const string _message = "The an instance of Comment is null!";
    public CommentNullException()
        : base(_message) { }
}
