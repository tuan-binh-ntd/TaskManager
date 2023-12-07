namespace TaskManager.Core.Exceptions;

public class MemberProjectNullException : Exception
{
    private const string _message = "The an instance of MemberProject is null!";
    public MemberProjectNullException()
        : base(_message) { }
}
