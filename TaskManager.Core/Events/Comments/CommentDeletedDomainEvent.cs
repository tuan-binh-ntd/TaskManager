namespace TaskManager.Core.Events.Comments;

public sealed class CommentDeletedDomainEvent(
    Guid issueId,
    Guid userId,
    string content
    )
    : IDomainEvent
{
    public Guid IssueId { get; private set; } = issueId;
    public Guid UserId { get; private set; } = userId;
    public string Content { get; private set; } = content;
}
