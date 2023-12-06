namespace TaskManager.Core.Exceptions;

public class IssueDetailNullException : Exception
{
    private const string _message = "The an instance of IssueDetail is null!";
    public IssueDetailNullException()
        : base(_message) { }
}
