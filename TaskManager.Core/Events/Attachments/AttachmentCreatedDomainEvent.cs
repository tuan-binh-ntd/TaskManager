namespace TaskManager.Core.Events.Attachments;

public sealed class AttachmentCreatedDomainEvent(
    string fileName,
    Guid issueId,
    Guid userId)
    : IDomainEvent
{
    public string FileName { get; private set; } = fileName;
    public Guid IssueId { get; private set; } = issueId;
    public Guid UserId { get; private set; } = userId;
}
