namespace TaskManager.Core.Entities;

public class IssueEvent : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    // Relationship
    public ICollection<NotificationIssueEvent>? NotificationIssueEvents { get; set; }

}
