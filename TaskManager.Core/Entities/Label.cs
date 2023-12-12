using TaskManager.Core.Core;

namespace TaskManager.Core.Entities;

public class Label : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    // Relationship
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    public ICollection<LabelIssue>? LabelIssues { get; set; }
}
