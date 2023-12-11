namespace TaskManager.Core.Exceptions;

public class NotificationIssueEventNullException : Exception
{
    private const string _message = "The an instance of NotificationIssueEvent is null!";
    public NotificationIssueEventNullException()
        : base(_message) { }
}
