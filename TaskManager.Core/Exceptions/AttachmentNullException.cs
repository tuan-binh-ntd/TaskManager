namespace TaskManager.Core.Exceptions;

public class AttachmentNullException : Exception
{
    private const string _message = "The an instance of Attachment is null!";
    public AttachmentNullException()
        : base(_message) { }
}
