namespace TaskManager.Core.Entities;

public class Notification : BaseEntity
{
    private Notification(string name, string description, List<NotificationIssueEvent> notificationIssueEvents, Guid projectId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        NotificationIssueEvents = notificationIssueEvents;
        ProjectId = projectId;
    }

    private Notification()
    {

    }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Relationship
    public ICollection<NotificationIssueEvent>? NotificationIssueEvents { get; set; }
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }

    public static Notification Create(string name, string description, List<NotificationIssueEvent> notificationIssueEvents, Guid projectId)
    {
        return new Notification(name, description, notificationIssueEvents, projectId);
    }
}
