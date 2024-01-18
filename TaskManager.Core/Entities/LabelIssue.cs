namespace TaskManager.Core.Entities;

public class LabelIssue : BaseEntity
{
    public Guid LabelId { get; set; }
    public Guid IssueId { get; set; }
    // Relationship
    public Label? Label { get; set; }
    public Issue? Issue { get; set; }
}
