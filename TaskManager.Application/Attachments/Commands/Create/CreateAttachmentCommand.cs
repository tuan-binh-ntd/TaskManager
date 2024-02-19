namespace TaskManager.Application.Attachments.Commands.Create;

public sealed class CreateAttachmentCommand(
    Guid issueId,
    IReadOnlyCollection<IFormFile> files,
    Guid userId
    ) : ICommand<Result<IReadOnlyCollection<AttachmentViewModel>>>
{
    public Guid IssueId { get; set; } = issueId;
    public IReadOnlyCollection<IFormFile> Files { get; set; } = files;
    public Guid UserId { get; set; } = userId;
}
