using TaskManager.Core.Core;

namespace TaskManager.Core.Entities;

public class Label : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    // Relationship
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    public ICollection<LabelIssue>? LabelIssues { get; set; }
}
