namespace TaskManager.Core.Exceptions;

public class ProjectConfigurationNullException : Exception
{
    private const string _message = "The an instance of ProjectConfiguration is null!";
    public ProjectConfigurationNullException()
        : base(_message) { }
}
