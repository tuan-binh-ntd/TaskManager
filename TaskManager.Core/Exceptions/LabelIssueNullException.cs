namespace TaskManager.Core.Exceptions;

public class LabelIssueNullException : Exception
{
    private const string _message = "The an instance of Issue is null!";
    public LabelIssueNullException()
        : base(_message) { }
}
