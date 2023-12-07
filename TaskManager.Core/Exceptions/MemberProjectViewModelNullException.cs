namespace TaskManager.Core.Exceptions;

public class MemberProjectViewModelNullException : Exception
{
    private const string _message = "The an instance of MemberProjectViewModel is null!";
    public MemberProjectViewModelNullException()
        : base(_message) { }
}
