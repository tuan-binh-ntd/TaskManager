namespace TaskManager.Core.Exceptions;

public class IssueNullException : Exception
{
    private const string _message = "The an instance of Issue is null!";
    public IssueNullException()
        : base(_message) { }
}
