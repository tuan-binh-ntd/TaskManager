namespace TaskManager.Core.Exceptions;

public class UserProjectNullException : Exception
{
    private const string _message = "The an instance of UserProject is null!";
    public UserProjectNullException()
        : base(_message) { }
}
