namespace TaskManager.Application.Attachments.Commands.Delete;

public sealed class DeleteAttachmentCommand(
    Guid attachmentId,
    Guid userId,
    Guid issueId
    ) : ICommand<Result<Guid>>
{
    public Guid AttachmentId { get; set; } = attachmentId;
    public Guid UserId { get; set; } = userId;
    public Guid IssueId { get; set; } = issueId;
}
