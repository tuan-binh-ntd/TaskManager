namespace TaskManager.Core.Entities;

public class VersionIssue : BaseEntity
{
    public Guid VersionId { get; set; }
    public Guid IssueId { get; set; }

    // Relationship
    public Version? Version { get; set; }
    public Issue? Issue { get; set; }
}
