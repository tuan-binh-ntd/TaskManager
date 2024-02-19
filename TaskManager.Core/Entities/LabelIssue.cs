namespace TaskManager.Core.Entities;

public class LabelIssue : BaseEntity
{
    private LabelIssue(Guid labelId, Guid issueId)
    {
        LabelId = labelId;
        IssueId = issueId;
    }

    private LabelIssue() { }

    public Guid LabelId { get; set; }
    public Guid IssueId { get; set; }
    // Relationship
    public Label? Label { get; set; }
    public Issue? Issue { get; set; }

    public static LabelIssue Create(Guid labelId, Guid issueId)
    {
        return new LabelIssue(labelId, issueId);
    }
}
