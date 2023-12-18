using TaskManager.Core.Core;

namespace TaskManager.Core.Entities;

public class Notification : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Relationship
    public ICollection<NotificationIssueEvent>? NotificationIssueEvents { get; set; }
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
}
