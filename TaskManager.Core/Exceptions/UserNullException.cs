namespace TaskManager.Core.Exceptions;

public class UserNullException : Exception
{
    private const string _message = "The an instance of User is null!";
    public UserNullException()
        : base(_message) { }
}
