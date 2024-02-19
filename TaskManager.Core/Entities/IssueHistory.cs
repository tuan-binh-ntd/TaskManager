namespace TaskManager.Core.Entities;

public class IssueHistory : BaseEntity
{
    private IssueHistory(string name, Guid creatorUserId, string content, Guid issueId)
    {
        Name = name;
        CreatorUserId = creatorUserId;
        Content = content;
        IssueId = issueId;
    }

    private IssueHistory(string name, Guid creatorUserId, Guid issueId)
    {
        Name = name;
        CreatorUserId = creatorUserId;
        IssueId = issueId;
    }

    private IssueHistory()
    {
    }

    public string Name { get; set; } = string.Empty;
    public Guid CreatorUserId { get; set; }
    public string Content { get; set; } = string.Empty;

    //Relationship
    public Guid IssueId { get; set; }
    public Issue? Issue { get; set; }

    public static IssueHistory Create(string name, Guid creatorUserId, string content, Guid issueId)
    {
        return new IssueHistory(name, creatorUserId, content, issueId);
    }

    public static IssueHistory Create(string name, Guid creatorUserId, Guid issueId)
    {
        return new IssueHistory(name, creatorUserId, issueId);
    }
}
