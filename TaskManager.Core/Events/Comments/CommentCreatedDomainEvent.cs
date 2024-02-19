namespace TaskManager.Core.Events.Comments;

public sealed class CommentCreatedDomainEvent(
    Guid issueId,
    Guid creatorUserId,
    string content
    )
    : IDomainEvent
{
    public Guid IssueId { get; private set; } = issueId;
    public Guid CreatorUserId { get; private set; } = creatorUserId;
    public string Content { get; private set; } = content;
}
