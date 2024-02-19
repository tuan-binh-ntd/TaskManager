namespace TaskManager.Core.Entities;

public class NotificationIssueEvent : BaseEntity
{
    private NotificationIssueEvent(bool allWatcher, bool currentAssignee, bool reporter, bool projectLead, Guid notificationId, Guid issueEventId)
    {
        Id = Guid.NewGuid();
        AllWatcher = allWatcher;
        CurrentAssignee = currentAssignee;
        Reporter = reporter;
        ProjectLead = projectLead;
        NotificationId = notificationId;
        IssueEventId = issueEventId;
    }

    private NotificationIssueEvent()
    {

    }

    public bool AllWatcher { get; set; }
    public bool CurrentAssignee { get; set; }
    public bool Reporter { get; set; }
    public bool ProjectLead { get; set; }

    //Relationship
    public Guid NotificationId { get; set; }
    public Notification? Notification { get; set; }
    public Guid IssueEventId { get; set; }
    public IssueEvent? IssueEvent { get; set; }

    public static NotificationIssueEvent Create(bool allWatcher, bool currentAssignee, bool reporter, bool projectLead, Guid notificationId, Guid issueEventId)
    {
        return new NotificationIssueEvent(allWatcher, currentAssignee, reporter, projectLead, notificationId, issueEventId);
    }
}
