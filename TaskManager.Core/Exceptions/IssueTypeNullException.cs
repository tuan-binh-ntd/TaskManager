namespace TaskManager.Core.Exceptions;

public class IssueTypeNullException : Exception
{
    private const string _message = "The an instance of IssueType is null!";
    public IssueTypeNullException()
        : base(_message) { }
}
