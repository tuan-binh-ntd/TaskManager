namespace TaskManager.Core.Entities;

public class NotificationIssueEvent : BaseEntity
{
    public bool AllWatcher { get; set; }
    public bool CurrentAssignee { get; set; }
    public bool Reporter { get; set; }
    public bool ProjectLead { get; set; }

    //Relationship
    public Guid NotificationId { get; set; }
    public Notification? Notification { get; set; }
    public Guid IssueEventId { get; set; }
    public IssueEvent? IssueEvent { get; set; }
}
