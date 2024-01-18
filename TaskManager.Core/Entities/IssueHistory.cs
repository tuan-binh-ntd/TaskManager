namespace TaskManager.Core.Entities;

public class IssueHistory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid CreatorUserId { get; set; }
    public string Content { get; set; } = string.Empty;

    //Relationship
    public Guid IssueId { get; set; }
    public Issue? Issue { get; set; }
}
