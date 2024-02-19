namespace TaskManager.Core.Entities;

public class Comment : BaseEntity
{
    private Comment(Guid creatorUserId, string content, Guid issueId)
    {
        CreatorUserId = creatorUserId;
        IsEdited = false;
        Content = content;
        IssueId = issueId;
    }

    private Comment()
    {
    }

    public Guid CreatorUserId { get; set; }
    public bool IsEdited { get; set; }
    public string Content { get; set; } = string.Empty;

    //Relationship
    public Guid IssueId { get; set; }
    public Issue? Issue { get; set; }

    public static Comment Create(Guid creatorUserId, string content, Guid issueId)
    {
        return new Comment(creatorUserId, content, issueId);
    }

    public void CommentCreated()
    {
        AddDomainEvent(new CommentCreatedDomainEvent(IssueId, CreatorUserId, Content));
    }

    public void CommentUpdated(string newContent)
    {
        Content = newContent;
        IsEdited = true;
    }

    public void CommentDeleted()
    {
        AddDomainEvent(new CommentDeletedDomainEvent(IssueId, CreatorUserId, Content));
    }
}
