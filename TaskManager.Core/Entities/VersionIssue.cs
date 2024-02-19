namespace TaskManager.Core.Entities;

public class VersionIssue : BaseEntity
{
    private VersionIssue(Guid versionId, Guid issueId)
    {
        VersionId = versionId;
        IssueId = issueId;
    }

    private VersionIssue() { }

    public Guid VersionId { get; set; }
    public Guid IssueId { get; set; }

    // Relationship
    public Version? Version { get; set; }
    public Issue? Issue { get; set; }

    public static VersionIssue Create(Guid versionId, Guid issueId)
    {
        return new VersionIssue(versionId, issueId);
    }
}
