namespace TaskManager.Core.Exceptions;

public class UserNotificationNullException : Exception
{
    private const string _message = "The an instance of UserNotification is null!";
    public UserNotificationNullException()
        : base(_message) { }
}