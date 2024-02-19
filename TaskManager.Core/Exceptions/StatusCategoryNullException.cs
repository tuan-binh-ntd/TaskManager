namespace TaskManager.Core.Exceptions;

public class StatusCategoryNullException : Exception
{
    private const string _message = "The an instance of StatusCategory is null!";
    public StatusCategoryNullException()
        : base(_message) { }
}

public class StatusNullException : Exception
{
    private const string _message = "The an instance of Status is null!";
    public StatusNullException()
        : base(_message) { }
}